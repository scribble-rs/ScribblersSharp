/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a generalized game message parser
    /// </summary>
    /// <typeparam name="T">Game message type</typeparam>
    public interface IGameMessageParser<T> : IBaseGameMessageParser where T : IReceiveGameMessageData
    {
        /// <summary>
        /// On game message parsed
        /// </summary>
        event GameMessageParsedDelegate<T> OnGameMessageParsed;

        /// <summary>
        /// On game message parse failed
        /// </summary>
        event GameMessageParseFailedDelegate<T> OnGameMessageParseFailed;
    }
}
