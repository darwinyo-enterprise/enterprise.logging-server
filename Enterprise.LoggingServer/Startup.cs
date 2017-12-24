using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.LoggingServer.Extension;
using Enterprise.Configurations.Constant;
using Enterprise.Extension.NetStandard;
using Enterprise.Constants.NetStandard;
using Enterprise.Enums.NetStandard;

namespace Enterprise.LoggingServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAuthorization();

            services.AddAuthentication(Config.APIAuthenticationSchema)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = Urls.AuthorizationServer_URL;
                options.RequireHttpsMetadata = true;
                options.ApiName = ApiNameEnum.LoggingServer.GetDescription();
            });

            services.RegisterDependencies(Configuration.GetConnectionString("Mongo"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCustomMiddlewares();

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
