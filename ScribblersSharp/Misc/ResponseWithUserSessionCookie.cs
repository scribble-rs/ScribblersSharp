using System;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Response with auth cookie
    /// </summary>
    /// <typeparam name="T">Response data type</typeparam>
    internal readonly struct ResponseWithUserSessionCookie<T> : IResponseWithUserSessionCookie<T> where T : IResponseData
    {
        /// <summary>
        /// Response
        /// </summary>
        public T Response { get; }

        /// <summary>
        /// USer session cookie
        /// </summary>
        public string UserSessionCookie { get; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (Response != null) &&
            (UserSessionCookie != null);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="response">Response</param>
        /// <param name="userSessionCookie">User session cookie</param>
        public ResponseWithUserSessionCookie(T response, string userSessionCookie)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (!response.IsValid)
            {
                throw new ArgumentException("Response is not valid.", nameof(response));
            }
            Response = response;
            UserSessionCookie = userSessionCookie ?? throw new ArgumentNullException(nameof(userSessionCookie));
        }
    }
}
