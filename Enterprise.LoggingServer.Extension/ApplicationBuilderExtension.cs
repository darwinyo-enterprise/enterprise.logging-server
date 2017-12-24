using Enterprise.Middlewares.NetStandard;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enterprise.LoggingServer.Extension
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            return app.UseMiddleware(typeof(WebAPIErrorHandlingMiddleware));
        }
    }
}
