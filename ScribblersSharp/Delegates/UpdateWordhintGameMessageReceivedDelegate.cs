using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// "update-wordhint" game message received delegate
    /// </summary>
    /// <param name="wordHints">Word hints</param>
    public delegate void UpdateWordhintGameMessageReceivedDelegate(IReadOnlyList<IWordHint> wordHints);
}
