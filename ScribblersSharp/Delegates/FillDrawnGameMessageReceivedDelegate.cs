using System.Drawing;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Fill drawn game message received delegate
    /// </summary>
    /// <param name="positionX">Fill position X</param>
    /// <param name="positionY">Fill position Y</param>
    /// <param name="color">Fill color</param>
    public delegate void FillDrawnGameMessageReceivedDelegate(float positionX, float positionY, Color color);
}
