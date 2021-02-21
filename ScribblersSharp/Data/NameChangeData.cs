using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes name change data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NameChangeData : IValidable
    {
        /// <summary>
        /// Player ID
        /// </summary>
        [JsonProperty("playerId")]
        public string PlayerID { get; set; }

        /// <summary>
        /// Player name
        /// </summary>
        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(PlayerID) &&
            !string.IsNullOrWhiteSpace(PlayerName);

    }
}
