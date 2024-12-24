using System;
using System.Diagnostics.CodeAnalysis;
using Lkq.Core.RulesRepo.Common;

namespace Lkq.Api.RulesRepo.Model
{
    /// <summary>
    /// VPSIApiResponse Model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RulesRepoApiResponse : IRulesRepoApiResponse
    {
        /// <summary>
        /// API Response Code
        /// </summary>
        /// <example>404</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// API Response Message
        /// </summary>
        /// <example>Not Found</example>
        public dynamic Message { get; set; }

        /// <summary>
        /// API Response Result
        /// </summary>
        public dynamic Result { get; set; }

        /// <summary>
        /// CreateAPISuccessResponse
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Return Success Response</returns>
        public RulesRepoApiResponse CreateAPISuccessResponse(dynamic result = null)
        {
            var type = result.GetType().Equals(typeof(string));

            return new RulesRepoApiResponse
            {
                StatusCode = Constants.OK,
                Message = type ? result : Constants.SUCCESS,
                Result = !type ? result : null 
            };
        }

        /// <summary>
        /// CreateAPIFailureResponse
        /// </summary>
        /// <returns>Return Response</returns>
        public RulesRepoApiResponse CreateAPIFailureResponse(string message = null, int statusCode = 0)
        {
             statusCode = statusCode == 0  && string.IsNullOrEmpty(message) ? Constants.SERVERERROR :
               statusCode != 0 ? statusCode : Constants.BADREQUEST;

            return new RulesRepoApiResponse
            {
                StatusCode = statusCode,
                Message = !string.IsNullOrEmpty(message) ? message : Constants.STATUSMESSAGE_SERVER_ERROR
            };
        }

        /// <summary>
        /// Created Response
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public RulesRepoApiResponse CreatedResponse(dynamic result)
        {
            return new RulesRepoApiResponse
            {
                StatusCode = Constants.CREATED,
                Message = Constants.STATUSMESSAGE_CREATED,
                Result = result
            };
        }
    }
}
