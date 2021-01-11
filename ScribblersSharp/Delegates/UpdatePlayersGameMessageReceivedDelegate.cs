using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// "update-players" game message received delegate
    /// </summary>
    /// <param name="players">Players</param>
    public delegate void UpdatePlayersGameMessageReceivedDelegate(IReadOnlyList<IPlayer> players);
}
