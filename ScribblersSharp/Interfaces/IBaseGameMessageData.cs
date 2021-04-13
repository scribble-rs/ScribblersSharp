/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents base game message data
    /// </summary>
    public interface IBaseGameMessageData : IValidable
    {
        /// <summary>
        /// Game message type
        /// </summary>
        string MessageType { get; }
    }
}
