using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Next turn data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NextTurnData
    {
        /// <summary>
        /// Round end time
        /// </summary>
        [JsonProperty("roundEndTime")]
        public ulong RoundEndTime { get; set; }

        /// <summary>
        /// Players
        /// </summary>
        [JsonProperty("players")]
        public PlayerData[] Players { get; set; }

        /// <summary>
        /// Round
        /// </summary>
        [JsonProperty("round")]
        public uint Round { get; set; }
    }
}
