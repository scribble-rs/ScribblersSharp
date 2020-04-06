using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Ready data class
    /// </summary>
    internal class ReadyData
    {
        /// <summary>
        /// Player ID
        /// </summary>
        [JsonPropertyName("playerId")]
        public uint PlayerID { get; set; }

        /// <summary>
        /// Is player drawing
        /// </summary>
        [JsonPropertyName("isDrawing")]
        public bool IsDrawing { get; set; }

        /// <summary>
        /// Round
        /// </summary>
        [JsonPropertyName("round")]
        public uint Round { get; set; }

        /// <summary>
        /// Maximal rounds
        /// </summary>
        [JsonPropertyName("maxRound")]
        public uint MaximalRounds { get; set; }

        /// <summary>
        /// Round end time
        /// </summary>
        [JsonPropertyName("roundEndTime")]
        public ulong RoundEndTime { get; set; }

        /// <summary>
        /// Word hints
        /// </summary>
        [JsonPropertyName("wordHints")]
        public WordHintData[] WordHints { get; set; }

        /// <summary>
        /// Players
        /// </summary>
        [JsonPropertyName("players")]
        public PlayerData[] Players { get; set; }

        /// <summary>
        /// Current drawing
        /// </summary>
        [JsonIgnore]
        public DrawCommand[] CurrentDrawing { get; set; }
    }
}
