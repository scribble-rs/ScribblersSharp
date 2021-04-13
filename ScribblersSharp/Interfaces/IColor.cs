/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a color
    /// </summary>
    public interface IColor
    {
        /// <summary>
        /// Red color component
        /// </summary>
        byte Red { get; }

        /// <summary>
        /// Green color component
        /// </summary>
        byte Green { get; }

        /// <summary>
        /// Blue color component
        /// </summary>
        byte Blue { get; }
    }
}
