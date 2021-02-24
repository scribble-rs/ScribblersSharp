using Newtonsoft.Json;
using ScribblersSharp.JSONConverters;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Player state enumerator
    /// </summary>
    [JsonConverter(typeof(PlayerStateJSONConverter))]
    public enum EPlayerState
    {
        /// <summary>
        /// Invalid player state
        /// </summary>
        Invalid,

        /// <summary>
        /// Standby
        /// </summary>
        Standby,

        /// <summary>
        /// Drawing
        /// </summary>
        Drawing,

        /// <summary>
        /// Guessing
        /// </summary>
        Guessing
    }
}
