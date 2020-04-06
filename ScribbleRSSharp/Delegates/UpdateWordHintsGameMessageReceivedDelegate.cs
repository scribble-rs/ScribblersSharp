using System.Collections.Generic;

/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Update word hints game message received delegate
    /// </summary>
    /// <param name="wordHints">Word hints</param>
    public delegate void UpdateWordHintsGameMessageReceivedDelegate(IReadOnlyList<WordHint> wordHints);
}
