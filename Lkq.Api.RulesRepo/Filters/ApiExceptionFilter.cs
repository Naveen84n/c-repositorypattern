using Lkq.Api.RulesRepo.Model;
using Lkq.Core.RulesRepo.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Lkq.Api.RulesRepo.Filters
{
    /// <summary>
    /// Custom Exception Filter
    /// </summary>
    public class ApiExceptionFilter : IAsyncExceptionFilter
    {

        private readonly ILogger<ApiExceptionFilter> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// On Exception Implementation
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public Task OnExceptionAsync(ExceptionContext context)
        {
            HttpStatusCode statusCode = (context.Exception as WebException != null &&
                     ((HttpWebResponse)(context.Exception as WebException).Response) != null) ?
                      ((HttpWebResponse)(context.Exception as WebException).Response).StatusCode
                      : GetErrorCode(context.Exception.GetType());

            var exception = context.Exception;
            context.HttpContext.Response.StatusCode = (int)statusCode;

            //Business exception-More generics for external world
            var error = new RulesRepoApiResponse()
            {
                StatusCode = (int)statusCode,
                Message = (int)statusCode == Constants.SERVERERROR ? Constants.STATUSMESSAGE_SERVER_ERROR : exception.Message
            };

            //Logs your technical exception with stack trace below
            _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName + " : " + exception.Message ?? exception.InnerException.Message);

            context.Result = new JsonResult(error);
            return Task.CompletedTask;
        }


        private static HttpStatusCode GetErrorCode(Type exceptionType)
        {
            if (Enum.TryParse(exceptionType.Name, out Exceptions tryParseResult))
            {
                return tryParseResult switch
                {
                    Exceptions.UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                    Exceptions.ArgumentException => HttpStatusCode.BadRequest,
                    Exceptions.ArgumentNullException => HttpStatusCode.BadRequest,
                    Exceptions.ArgumentOutOfRangeException => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError,
                };
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }

    }
}
