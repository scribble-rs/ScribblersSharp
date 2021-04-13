using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "your-turn" game message has been received
    /// </summary>
    /// <param name="words">Words</param>
    public delegate void YourTurnGameMessageReceivedDelegate(IReadOnlyList<string> words);
}
