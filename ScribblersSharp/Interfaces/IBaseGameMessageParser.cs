/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a base game message
    /// </summary>
    public interface IBaseGameMessageParser
    {
        /// <summary>
        /// Game message type
        /// </summary>
        string MessageType { get; }

        /// <summary>
        /// Parses incoming game message
        /// </summary>
        /// <param name="json">JSON</param>
        void ParseMessage(string json);
    }
}
