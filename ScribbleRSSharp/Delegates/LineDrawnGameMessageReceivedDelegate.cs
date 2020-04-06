using System.Drawing;
using System.Numerics;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Line drawn game message received delegate
    /// </summary>
    /// <param name="from">Line from</param>
    /// <param name="to">Line to</param>
    /// <param name="color">Line color</param>
    /// <param name="lineWidth">Line width</param>
    public delegate void LineDrawnGameMessageReceivedDelegate(Vector2 from, Vector2 to, Color color, float lineWidth);
}
