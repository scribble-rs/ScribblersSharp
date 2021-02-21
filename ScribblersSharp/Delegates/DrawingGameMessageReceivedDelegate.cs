using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "drawing" game message has been received.
    /// </summary>
    /// <param name="currentDrawing">Current drawing</param>
    public delegate void DrawingGameMessageReceivedDelegate(IReadOnlyList<IDrawCommand> currentDrawing);
}
