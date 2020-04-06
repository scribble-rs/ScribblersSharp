using System.Drawing;
using System.Numerics;

/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
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
        /// Draw from
        /// </summary>
        public Vector2 From { get; }

        /// <summary>
        /// Draw to (used for lines)
        /// </summary>
        public Vector2 To { get; }

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
        /// <param name="from">Draw from</param>
        /// <param name="to">Draw to (used for lines)</param>
        /// <param name="color">Draw color</param>
        /// <param name="lineWidth">Line width (used for lines)</param>
        internal DrawCommand(EDrawCommandType type, Vector2 from, Vector2 to, Color color, float lineWidth)
        {
            Type = type;
            From = from;
            To = to;
            Color = color;
            LineWidth = lineWidth;
        }
    }
}
