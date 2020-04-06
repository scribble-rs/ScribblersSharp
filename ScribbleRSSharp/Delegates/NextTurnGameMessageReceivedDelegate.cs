using System.Collections.Generic;

/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Next turn game message received delegate
    /// </summary>
    /// <param name="players">Players</param>
    /// <param name="round">Round</param>
    /// <param name="roundEndTime">Round end time</param>
    public delegate void NextTurnGameMessageReceivedDelegate(IReadOnlyList<Player> players, uint round, ulong roundEndTime);
}
