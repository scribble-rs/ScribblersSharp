using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Your turn game message received delegate
    /// </summary>
    /// <param name="words">Words</param>
    public delegate void YourTurnGameMessageReceivedDelegate(IReadOnlyList<string> words);
}
