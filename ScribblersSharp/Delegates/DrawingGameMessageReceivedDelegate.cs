using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// "drawing" game message received delegate
    /// </summary>
    public delegate void DrawingGameMessageReceivedDelegate(IReadOnlyList<IDrawCommand> drawCommands);
}
