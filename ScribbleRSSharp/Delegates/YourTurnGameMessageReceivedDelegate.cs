using System.Collections.Generic;

/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Your turn game message received delegate
    /// </summary>
    /// <param name="words">Words</param>
    public delegate void YourTurnGameMessageReceivedDelegate(IReadOnlyList<string> words);
}
