using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace TestRulesRepo.Server
{
    /// <summary>
    /// Response from the authentication system
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthResponse
    {
        /// <summary>
        /// True or false if the call was successful
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Returns this object as a successful response
        /// </summary>
        [JsonIgnore]
        public AuthResponseSuccess AsSuccess => (AuthResponseSuccess) this;

        /// <summary>
        /// Returns this object as a failed response
        /// </summary>
        [JsonIgnore]
        public AuthResponseFailed AsFailed => (AuthResponseFailed)this;
    }

    /// <summary>
    /// Failed auth response
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthResponseFailed : AuthResponse
    {
        /// <summary>
        /// Error Type
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; }
        
        /// <summary>
        /// Error Description
        /// </summary>
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }

        #region cTor
        /// <summary>
        /// Creates default instance
        /// </summary>
        public AuthResponseFailed()
        {
            Success = false;
        }

        /// <summary>
        /// Creates instance with values
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        public AuthResponseFailed(AuthError error, string message) : this()
        {
            Error = error.ToString();
            ErrorDescription = message;
        }
        #endregion
    }

    /// <summary>
    /// Successful auth response
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthResponseSuccess : AuthResponse
    {
        /// <summary>
        /// Access token used to interact with API, submitted in the header of requests
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Type of token returned
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Time frame token remains good for
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Scope this token is good for
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        #region cTor
        /// <summary>
        /// Creates default instance
        /// </summary>
        public AuthResponseSuccess()
        {
            Success = true;
        }

        /// <summary>
        /// Creates instance with values
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="tokenType"></param>
        /// <param name="expiresIn"></param>
        /// <param name="scope"></param>
        public AuthResponseSuccess(string accessToken, string tokenType, int expiresIn, string scope) : this()
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
            Scope = scope;
        }
        #endregion
    }

    /// <summary>
    /// Authentication errors
    /// </summary>
    [Serializable]
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AuthError
    {
        /// <summary>
        /// Invalid Client
        /// </summary>
        InvalidClient,
        /// <summary>
        /// Invalid Scope
        /// </summary>
        InvalidScope,
        /// <summary>
        /// Invalid Grant Type
        /// </summary>
        InvalidGrantType,
        /// <summary>
        /// Grant Type was not allowed
        /// </summary>
        GrantTypeNotAllowed,
        /// <summary>
        /// Authentication Failed
        /// </summary>
        AuthenticationFailed
    }
}
