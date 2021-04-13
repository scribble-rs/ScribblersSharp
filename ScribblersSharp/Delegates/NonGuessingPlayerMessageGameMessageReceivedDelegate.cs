/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "non-guessing-player-message" game message has been received
    /// </summary>
    /// <param name="author">Author</param>
    /// <param name="content">Content</param>
    public delegate void NonGuessingPlayerMessageGameMessageReceivedDelegate(IPlayer author, string content);
}
