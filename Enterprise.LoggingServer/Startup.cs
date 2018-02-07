using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.LoggingServer.Extension;
using Enterprise.Constants.NetStandard;
using Microsoft.AspNetCore.Mvc.Authorization;
using Enterprise.Extension.NetStandard;
using Enterprise.Helpers.NetStandard;

namespace Enterprise.LoggingServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Required in all applications
            InitializeStartupHelper.InitializeStaticFields(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("read_only_access_policy", builder =>
                {
                    builder.RequireScope(LoggingServerScopes.read_only_access, LoggingServerScopes.full_access);
                });
                opt.AddPolicy("write_access_policy", builder =>
                {
                    builder.RequireScope(LoggingServerScopes.write_access, LoggingServerScopes.full_access);
                });
                opt.AddPolicy("delete_access_policy", builder =>
                {
                    builder.RequireScope(LoggingServerScopes.delete_access, LoggingServerScopes.full_access);
                });
            });

            services.AddAuthentication(Config.BearerSchema)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Urls.AuthorizationServer_URL;
                    options.RequireHttpsMetadata = true;
                    options.ApiName = ApiNames.loggingserver;
                    options.ApiSecret = APISecrets.loggingserver;
                });

            services.RegisterDependencies(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseCustomExceptionMiddlewares();
            }
            else
            {
                //app.UseCustomExceptionMiddlewares();
                app.UseExceptionHandler();
            }

            app.UseMvc();
        }
    }
}
