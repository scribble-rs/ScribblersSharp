using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes lobby view data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LobbyViewData
    {
        /// <summary>
        /// Lobby ID
        /// </summary>
        [JsonProperty("lobbyId")]
        public string LobbyID { get; set; }

        /// <summary>
        /// Player count
        /// </summary>
        [JsonProperty("playerCount")]
        public uint PlayerCount { get; set; }

        /// <summary>
        /// Maximal player count
        /// </summary>
        [JsonProperty("maxPlayers")]
        public uint MaximalPlayerCount { get; set; }

        /// <summary>
        /// Current round
        /// </summary>
        [JsonProperty("round")]
        public uint CurrentRound { get; set; }

        /// <summary>
        /// Maximal round count
        /// </summary>
        [JsonProperty("rounds")]
        public uint MaximalRoundCount { get; set; }

        /// <summary>
        /// Drawing time
        /// </summary>
        [JsonProperty("drawingTime")]
        public uint DrawingTime { get; set; }

        /// <summary>
        /// Is using custom words
        /// </summary>
        [JsonProperty("customWords")]
        public bool IsUsingCustomWords { get; set; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        [JsonProperty("votekick")]
        public bool IsVotekickingEnabled { get; set; }

        /// <summary>
        /// Maximal clients per IP count
        /// </summary>
        [JsonProperty("maxClientsPerIp")]
        public uint MaximalClientsPerIPCount { get; set; }

        /// <summary>
        /// Language
        /// </summary>
        [JsonProperty("wordpack")]
        public ELanguage Language { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(LobbyID) &&
            (MaximalPlayerCount > 1U) &&
            (MaximalRoundCount > 0U) &&
            (MaximalClientsPerIPCount > 0U);
    }
}
