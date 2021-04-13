/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a game message has been successfully parsed
    /// </summary>
    /// <typeparam name="T">Game message type</typeparam>
    /// <param name="gameMessage">Game message</param>
    /// <param name="json">JSON</param>
    public delegate void GameMessageParsedDelegate<T>(T gameMessage, string json) where T : IReceiveGameMessageData;
}
