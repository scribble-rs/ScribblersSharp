/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// "message" game message received delegate
    /// </summary>
    /// <param name="author">Author</param>
    /// <param name="content">Content</param>
    public delegate void MessageGameMessageReceivedDelegate(IPlayer author, string content);
}
