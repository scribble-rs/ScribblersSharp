/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a non-meaningful game message has been received
    /// </summary>
    /// <param name="message">Game message</param>
    /// <param name="json">Game message JSON</param>
    public delegate void UnknownGameMessageReceivedDelegate(IBaseGameMessageData gameMessage, string json);
}
