using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Update word hints game message received delegate
    /// </summary>
    /// <param name="wordHints">Word hints</param>
    public delegate void UpdateWordHintsGameMessageReceivedDelegate(IReadOnlyList<IWordHint> wordHints);
}
