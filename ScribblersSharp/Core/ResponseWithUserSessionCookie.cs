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
    internal struct ResponseWithUserSessionCookie<T> where T : IResponseData
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
            if (userSessionCookie == null)
            {
                throw new ArgumentNullException(nameof(userSessionCookie));
            }
            Response = response;
            UserSessionCookie = userSessionCookie;
        }
    }
}
