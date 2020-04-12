using System.Drawing;
using System.Numerics;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Draw command structure
    /// </summary>
    public struct DrawCommand
    {
        /// <summary>
        /// Draw command type
        /// </summary>
        public EDrawCommandType Type { get; }

        /// <summary>
        /// Draw from X
        /// </summary>
        public float FromX { get; }

        /// <summary>
        /// Draw from Y
        /// </summary>
        public float FromY { get; }

        /// <summary>
        /// Draw to X (used for lines)
        /// </summary>
        public float ToX { get; }

        /// <summary>
        /// Draw to Y (used for lines)
        /// </summary>
        public float ToY { get; }

        /// <summary>
        /// Draw color
        /// </summary>
        public Color Color;

        /// <summary>
        /// Line width (used for lines)
        /// </summary>
        public float LineWidth { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Draw command type</param>
        /// <param name="fromX">Draw from X</param>
        /// <param name="fromY">Draw from Y</param>
        /// <param name="toX">Draw to X (used for lines)</param>
        /// <param name="toY">Draw to Y (used for lines)</param>
        /// <param name="color">Draw color</param>
        /// <param name="lineWidth">Line width (used for lines)</param>
        internal DrawCommand(EDrawCommandType type, float fromX, float fromY, float toX, float toY, Color color, float lineWidth)
        {
            Type = type;
            FromX = fromX;
            FromY = fromY;
            ToX = toX;
            ToY = toY;
            Color = color;
            LineWidth = lineWidth;
        }
    }
}
