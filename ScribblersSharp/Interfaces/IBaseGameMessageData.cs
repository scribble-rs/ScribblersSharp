/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Base game message data interface
    /// </summary>
    internal interface IBaseGameMessageData : IValidable
    {
        /// <summary>
        /// Game message type
        /// </summary>
        string Type { get; }
    }
}
