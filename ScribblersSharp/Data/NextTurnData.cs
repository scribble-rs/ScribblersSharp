using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes next turn data
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
        public List<PlayerData> Players { get; set; }

        /// <summary>
        /// Round
        /// </summary>
        [JsonProperty("round")]
        public uint Round { get; set; }

        /// <summary>
        /// Previous word
        /// </summary>
        [JsonProperty("previousWord")]
        public string PreviousWord { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            Protection.IsValid(Players) &&
            Protection.AreUnique(Players, (left, right) => left.ID != right.ID);
    }
}
