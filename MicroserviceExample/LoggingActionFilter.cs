using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MicroserviceExample
{
    public class LoggingActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
        {
            // Read the Request-Id header so we can assign it to the activity.
            string operationId = Guid.NewGuid().ToString();
            string parentOperationId = context.HttpContext.Request.Headers["Request-Id"];

            if (!string.IsNullOrEmpty( parentOperationId ))
            {
                operationId = parentOperationId + "." + operationId;
            }


            using (LogContext.PushProperty( "OperationId", operationId ))
            {
                await next();
            }

        }
       
    }
}