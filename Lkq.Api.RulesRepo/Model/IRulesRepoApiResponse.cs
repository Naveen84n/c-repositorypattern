namespace Lkq.Api.RulesRepo.Model
{
    /// <summary>
    /// VPSI API Response Interface
    /// </summary>
    public interface IRulesRepoApiResponse
    {
        /// <summary>
        /// Success Response
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        RulesRepoApiResponse CreateAPISuccessResponse(dynamic result);

        /// <summary>
        /// Failure Response
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        RulesRepoApiResponse CreateAPIFailureResponse(string message = null, int statusCode = 0);

        /// <summary>
        /// Created Response
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        RulesRepoApiResponse CreatedResponse(dynamic result);

    }
}
