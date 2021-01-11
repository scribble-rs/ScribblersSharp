using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// "your-turn" game message received delegate
    /// </summary>
    /// <param name="words">Words</param>
    public delegate void YourTurnGameMessageReceivedDelegate(IReadOnlyList<string> words);
}
