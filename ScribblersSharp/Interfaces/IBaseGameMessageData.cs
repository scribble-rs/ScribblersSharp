/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Base game message data interface
    /// </summary>
    public interface IBaseGameMessageData : IValidable
    {
        /// <summary>
        /// Game message type
        /// </summary>
        string MessageType { get; }
    }
}
