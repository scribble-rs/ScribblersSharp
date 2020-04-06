using System.Drawing;
using System.Numerics;

/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Fill drawn game message received delegate
    /// </summary>
    /// <param name="position">Fill position</param>
    /// <param name="color">Fill color</param>
    public delegate void FillDrawnGameMessageReceivedDelegate(Vector2 position, Color color);
}
