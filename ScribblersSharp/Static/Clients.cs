/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A class used to instantiate Scribble.rs clients
    /// </summary>
    public static class Clients
    {
        /// <summary>
        /// Creates a new Scribble.rs client
        /// </summary>
        /// <param name="host">Scribble.rs host</param>
        /// <param name="userSessionID">User session ID</param>
        /// <returns>Scribble.rs client</returns>
        public static IScribblersClient Create(string host, string userSessionID) => Create(host, userSessionID, true, false);

        /// <summary>
        /// Creates a new Scribble.rs client
        /// </summary>
        /// <param name="host">Scribble.rs host</param>
        /// <param name="userSessionID">User session ID</param>
        /// <param name="isUsingSecureProtocols">Is using secure protocols</param>
        /// <param name="isAllowedToUseInsecureConnections">Is allowed to use insecure connections</param>
        /// <returns>Scribble.rs client</returns>
        public static IScribblersClient Create(string host, string userSessionID, bool isUsingSecureProtocols, bool isAllowedToUseInsecureConnections) => new ScribblersClient(host, userSessionID, isUsingSecureProtocols, isAllowedToUseInsecureConnections);
    }
}
