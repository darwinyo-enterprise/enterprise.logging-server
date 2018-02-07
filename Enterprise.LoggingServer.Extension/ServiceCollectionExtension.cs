using Enterprise.Constants.NetStandard;
using Enterprise.LoggingServer.DataLayers.Mongo;
using Enterprise.LoggingServer.Interfaces.Mongo;
using Enterprise.LoggingServer.Repository.Mongo;
using Enterprise.LoggingServer.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enterprise.LoggingServer.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterDependencies(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<LogMongoContext>(x => new LogMongoContext(configuration.GetConnectionString(ConfigurationNames.LoggingConnection)));

            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IDebugLogRepository, DebugLogRepository>();

            services.AddScoped<ILogMongoUnitOfWork, LogMongoUnitOfWork>();
        }
    }
}
