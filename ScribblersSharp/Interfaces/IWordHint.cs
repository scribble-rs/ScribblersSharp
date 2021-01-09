/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a word hint
    /// </summary>
    public interface IWordHint
    {
        /// <summary>
        /// Character
        /// </summary>
        char Character { get; }

        /// <summary>
        /// Underline
        /// </summary>
        bool Underline { get; }
    }
}
