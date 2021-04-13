/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when parsing a game message has failed
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    /// <param name="expectedMessageType">Expected message type</param>
    /// <param name="message">Game message</param>
    /// <param name="json">Game message JSON</param>
    public delegate void GameMessageParseFailedDelegate<T>(string expectedMessageType, T message, string json) where T : IReceiveGameMessageData;
}
