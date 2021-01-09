using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Ready game message received delegate
    /// </summary>
    /// <param name="playerID">Player ID</param>
    /// <param name="isDrawing">Is player drawing</param>
    /// <param name="ownerID">Owner ID</param>
    /// <param name="round">Round</param>
    /// <param name="maximalRounds">Maximal rounds</param>
    /// <param name="roundEndTime">Round end time</param>
    /// <param name="wordHints">Word hints</param>
    /// <param name="players">Players</param>
    /// <param name="currentDrawing">Current drawing</param>
    /// <param name="gameState">Game state</param>
    public delegate void ReadyGameMessageReceivedDelegate(string playerID, bool isDrawing, string ownerID, uint round, uint maximalRounds, long roundEndTime, IReadOnlyList<IWordHint> wordHints, IReadOnlyList<IPlayer> players, IReadOnlyList<IDrawCommand> currentDrawing, EGameState gameState);
}
