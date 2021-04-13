/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a draw command
    /// </summary>
    public interface IDrawCommand : IValidable
    {
        /// <summary>
        /// Draw command type
        /// </summary>
        EDrawCommandType Type { get; }

        /// <summary>
        /// Draw from X
        /// </summary>
        float FromX { get; }

        /// <summary>
        /// Draw from Y
        /// </summary>
        float FromY { get; }

        /// <summary>
        /// Draw to X (used for lines)
        /// </summary>
        float ToX { get; }

        /// <summary>
        /// Draw to Y (used for lines)
        /// </summary>
        float ToY { get; }

        /// <summary>
        /// Draw color
        /// </summary>
        Color Color { get; }

        /// <summary>
        /// Line width (used for lines)
        /// </summary>
        float LineWidth { get; }
    }
}
