/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Game state enumerator
    /// </summary>
    public enum EGameState
    {
        /// <summary>
        /// Invalid game state
        /// </summary>
        Invalid,

        /// <summary>
        /// Game has not started yet
        /// </summary>
        Unstarted,

        /// <summary>
        /// Game is over
        /// </summary>
        GameOver,

        /// <summary>
        /// Game is still going
        /// </summary>
        Ongoing
    }
}
