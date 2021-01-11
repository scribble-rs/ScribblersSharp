using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// "next-turn" game message received delegate
    /// </summary>
    /// <param name="players">Players</param>
    /// <param name="round">Round</param>
    /// <param name="roundEndTime">Round end time</param>
    public delegate void NextTurnGameMessageReceivedDelegate(IReadOnlyList<IPlayer> players, uint round, ulong roundEndTime);
}
