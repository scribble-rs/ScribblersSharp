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
        /// Is player drawing
        /// </summary>
        [JsonProperty("isDrawing")]
        public bool IsDrawing { get; set; }

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
        [JsonProperty("maxRound")]
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
        /// Current drawing
        /// </summary>
        [JsonIgnore]
        public List<DrawCommand> CurrentDrawing { get; set; }

        /// <summary>
        /// Game state
        /// </summary>
        [JsonProperty("state")]
        [JsonConverter(typeof(GameStateJSONConverter))]
        public EGameState GameState { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (PlayerID != null) &&
            (OwnerID != null) &&
            Protection.IsValid(WordHints) &&
            Protection.IsValid(Players) &&
            (GameState != EGameState.Unknown);
    }
}
