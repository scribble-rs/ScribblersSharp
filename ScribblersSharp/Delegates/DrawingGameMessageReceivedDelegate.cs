using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// "drawing" game message received delegate
    /// </summary>
    /// <param name="currentDrawing">Current drawing</param>
    public delegate void DrawingGameMessageReceivedDelegate(IReadOnlyList<IDrawCommand> currentDrawing);
}
