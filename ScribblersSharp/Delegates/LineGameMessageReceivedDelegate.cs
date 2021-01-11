using System.Drawing;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// "line" game message received delegate
    /// </summary>
    /// <param name="fromX">Line from X</param>
    /// <param name="fromY">Line from Y</param>
    /// <param name="toX">Line to X</param>
    /// <param name="toY">Line to Y</param>
    /// <param name="color">Line color</param>
    /// <param name="lineWidth">Line width</param>
    public delegate void LineGameMessageReceivedDelegate(float fromX, float fromY, float toX, float toY, Color color, float lineWidth);
}
