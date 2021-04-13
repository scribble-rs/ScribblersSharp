using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes lobby settings change data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LobbySettingsChangeData : IValidable
    {
        /// <summary>
        /// Maximal player count
        /// </summary>
        [JsonProperty("maxPlayers")]
        public uint MaximalPlayerCount { get; set; }

        /// <summary>
        /// Is lobby public
        /// </summary>
        [JsonProperty("public")]
        public bool IsLobbyPublic { get; set; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        [JsonProperty("enableVotekick")]
        public bool IsVotekickingEnabled { get; set; }

        /// <summary>
        /// Custom words chance
        /// </summary>
        [JsonProperty("customWordsChance")]
        public uint CustomWordsChance { get; set; }

        /// <summary>
        /// Allowed clients per IP count
        /// </summary>
        [JsonProperty("clientsPerIpLimit")]
        public uint AllowedClientsPerIPCount { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (MaximalPlayerCount > 0U) &&
            (CustomWordsChance <= 100U) &&
            (AllowedClientsPerIPCount > 0U);
    }
}
