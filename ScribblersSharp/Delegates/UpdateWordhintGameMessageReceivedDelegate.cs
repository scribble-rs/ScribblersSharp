using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "update-wordhint" game message has been received.
    /// </summary>
    /// <param name="wordHints">Word hints</param>
    public delegate void UpdateWordhintGameMessageReceivedDelegate(IReadOnlyList<IWordHint> wordHints);
}
