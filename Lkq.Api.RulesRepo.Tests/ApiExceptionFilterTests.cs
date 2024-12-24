using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Lkq.Api.RulesRepo.Filters;


namespace Lkq.Api.RulesRepo.Tests.Controller
{
    public class ApiExceptionFilterTests
    {

        /// <summary>
        /// Exception Tests
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(400, Exceptions.ArgumentException)]
        [InlineData(400, Exceptions.ArgumentNullException)]
        [InlineData(400, Exceptions.ArgumentOutOfRangeException)]
        [InlineData(401, Exceptions.UnauthorizedAccessException)]
        [InlineData(500, null)]
        public async Task OnExceptionAsync_ReturnsHttpStatusCode(int statusCode, Exceptions? exceptionType)
        {

            //Arrange  
            var _mocklogger = new Mock<ILogger<ApiExceptionFilter>>();
            ExceptionContext exceptionContext= GetExceptionContext(exceptionType);

            //Act
            var filter = new ApiExceptionFilter(_mocklogger.Object);
            await filter.OnExceptionAsync(exceptionContext);

            //Assert
            Assert.Equal(statusCode, exceptionContext.HttpContext.Response.StatusCode);
        }

        private static ExceptionContext GetExceptionContext(Exceptions? exceptionType)
        {
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };
            dynamic exception = exceptionType switch
            {
                Exceptions.ArgumentOutOfRangeException => new ArgumentOutOfRangeException(),
                Exceptions.UnauthorizedAccessException => new UnauthorizedAccessException(),
                Exceptions.ArgumentException => new ArgumentException(),
                Exceptions.ArgumentNullException => new ArgumentNullException(),
                _ => new Exception(),
            };

            return new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = exception
            };
        }
    }
}
