using Newtonsoft.Json;
using ScribblersSharp.JSONConverters;
using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Ready data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ReadyData : IValidable
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
        /// Is player allowed to draw
        /// </summary>
        [JsonProperty("allowDrawing")]
        public bool IsPlayerAllowedToDraw { get; set; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        [JsonProperty("votekickEnabled")]
        public bool IsVotekickingEnabled { get; set; }

        /// <summary>
        /// Game state
        /// </summary>
        [JsonProperty("gameState")]
        [JsonConverter(typeof(GameStateJSONConverter))]
        public EGameState GameState { get; set; }

        /// <summary>
        /// Owner ID
        /// </summary>
        [JsonProperty("ownerId")]
        public string OwnerID { get; set; }

        /// <summary>
        /// Round
        /// </summary>
        [JsonProperty("round")]
        public uint Round { get; set; }

        /// <summary>
        /// Maximal rounds
        /// </summary>
        [JsonProperty("maxRounds")]
        public uint MaximalRounds { get; set; }

        /// <summary>
        /// Round end time
        /// </summary>
        [JsonProperty("roundEndTime")]
        public long RoundEndTime { get; set; }

        /// <summary>
        /// Word hints
        /// </summary>
        [JsonProperty("wordHints")]
        public List<WordHintData> WordHints { get; set; }

        /// <summary>
        /// Players
        /// </summary>
        [JsonProperty("players")]
        public List<PlayerData> Players { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (PlayerID != null) &&
            !string.IsNullOrWhiteSpace(PlayerName) &&
            (GameState != EGameState.Unknown) &&
            (OwnerID != null) &&
            Protection.IsValid(Players) &&
            Protection.IsContained(Players, (player) => (player.ID == PlayerID) && (player.Name == PlayerName)) &&
            Protection.IsContained(Players, (player) => player.ID == OwnerID) &&
            Protection.AreUnique(Players, (left, right) => left.ID != right.ID);
    }
}
