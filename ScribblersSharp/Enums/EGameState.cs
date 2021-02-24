using Newtonsoft.Json;
using ScribblersSharp.JSONConverters;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Game state enumerator
    /// </summary>
    [JsonConverter(typeof(GameStateJSONConverter))]
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
