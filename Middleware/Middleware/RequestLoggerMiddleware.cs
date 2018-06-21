using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware
{
    public class RequestLoggerMiddleware
    {
        public readonly RequestDelegate _next;
        public readonly ILogger _logger;

        public RequestLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggerMiddleware>();
        }

        public async Task Invoke(HttpContext context)//手工高亮
        {
            _logger.LogInformation("Handling request: " + context.Request.Path);
            //await _next.Invoke(context);
            await context.Response.WriteAsync("This is RequestLogger");

            _logger.LogInformation("Finished handling request.");
        }
    }
}
