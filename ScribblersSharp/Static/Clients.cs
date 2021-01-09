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
        /// <returns>Scribble.rs client</returns>
        public static IScribblersClient Create(string host) => new ScribblersClient(host);
    }
}
