using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "update-players" game message has been received.
    /// </summary>
    /// <param name="players">Players</param>
    public delegate void UpdatePlayersGameMessageReceivedDelegate(IReadOnlyDictionary<string, IPlayer> players);
}
