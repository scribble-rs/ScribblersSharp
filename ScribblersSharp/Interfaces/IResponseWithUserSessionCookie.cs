/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a response with user session cookie set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IResponseWithUserSessionCookie<T> : IValidable where T : IResponseData
    {
        /// <summary>
        /// Response
        /// </summary>
        T Response { get; }

        /// <summary>
        /// USer session cookie
        /// </summary>
        string UserSessionCookie { get; }
    }
}
