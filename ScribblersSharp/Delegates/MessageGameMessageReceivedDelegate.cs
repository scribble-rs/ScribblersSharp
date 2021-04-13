/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "message" game message has been received
    /// </summary>
    /// <param name="author">Author</param>
    /// <param name="content">Content</param>
    public delegate void MessageGameMessageReceivedDelegate(IPlayer author, string content);
}
