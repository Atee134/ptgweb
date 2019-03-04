using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Ptg.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PtgWeb.MiddleWares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;

            List<string> errorMessages = new List<string>();
            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                foreach (Exception e in aggregateException.InnerExceptions)
                {
                    errorMessages.Add(e.Message);
                }

                code = HttpStatusCode.InternalServerError;
            }
            else if (exception is PtgInvalidActionException)
            {
                code = HttpStatusCode.BadRequest;
            }
            else if (exception is PtgNotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            //else if (exception is ApplicationException) code = HttpStatusCode.InternalServerError;
            else code = HttpStatusCode.InternalServerError;

            string result;
            if (errorMessages.Count > 0)
            {
                result = JsonConvert.SerializeObject(new { error = errorMessages });
            }
            else
            {
                result = JsonConvert.SerializeObject(new { error = new List<string> { exception.Message } });
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
