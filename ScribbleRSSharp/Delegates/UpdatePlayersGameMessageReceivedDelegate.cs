using System.Collections.Generic;

/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Update players game message received delegate
    /// </summary>
    /// <param name="players">Players</param>
    public delegate void UpdatePlayersGameMessageReceivedDelegate(IReadOnlyList<Player> players);
}
