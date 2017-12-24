using Enterprise.LoggingServer.DataLayers.Mongo;
using Enterprise.LoggingServer.Interfaces.Mongo;
using Enterprise.LoggingServer.Repository.Mongo;
using Enterprise.LoggingServer.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enterprise.LoggingServer.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<LogMongoContext>(x => new LogMongoContext(connectionString));

            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IDebugLogRepository, DebugLogRepository>();

            services.AddScoped<ILogMongoUnitOfWork, LogMongoUnitOfWork>();
        }
    }
}
