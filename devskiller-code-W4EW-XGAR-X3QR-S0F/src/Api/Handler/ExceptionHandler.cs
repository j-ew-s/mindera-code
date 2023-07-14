using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Net;
using Api.Models;
using CrossCutting.CustonException;

namespace Api.Handler
{
	public class ExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var result = GetHttpResponse(context);

            context.Result = new JsonResult(result);

            context.HttpContext.Response.StatusCode = GetHttpStatusCode(context);

            base.OnException(context);
        }

        private static int GetHttpStatusCode(ExceptionContext context)
        {
            var type = context.Exception;

            if (type is NotExistException)
            {
                return (int)HttpStatusCode.NotFound;
            }
            if(type is RequiredException)
            {
                return (int)HttpStatusCode.BadRequest;
            }
            else
            {
                return (int)HttpStatusCode.InternalServerError;
            }
        }

        private static Response<List<string>> GetHttpResponse(ExceptionContext context)
        {
            var messages = new List<string>();
            var innerMessage = context.Exception;

            do
            {
                if (!string.IsNullOrEmpty(innerMessage.Message))
                {
                    messages.Add(innerMessage.Message);
                }

                innerMessage = innerMessage.InnerException;

            } while (innerMessage != null);


            return new Response<List<string>>(messages);
        }
    }
}

