using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Ready data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ReadyData
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
        public WordHintData[] WordHints { get; set; }

        /// <summary>
        /// Players
        /// </summary>
        [JsonProperty("players")]
        public PlayerData[] Players { get; set; }

        /// <summary>
        /// Current drawing
        /// </summary>
        [JsonIgnore]
        public DrawCommand[] CurrentDrawing { get; set; }
    }
}
