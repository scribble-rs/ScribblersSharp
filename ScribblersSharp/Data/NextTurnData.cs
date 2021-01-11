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
    internal class NextTurnData : IValidable
    {
        /// <summary>
        /// Round end time
        /// </summary>
        [JsonProperty("roundEndTime")]
        public long RoundEndTime { get; set; }

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

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            Protection.IsValid(Players) &&
            Protection.AreUnique(Players, (left, right) => left.ID != right.ID);
    }
}
