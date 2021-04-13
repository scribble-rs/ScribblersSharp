using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes player data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class PlayerData : IValidable
    {
        /// <summary>
        /// Player ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Player name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Player score
        /// </summary>
        [JsonProperty("score")]
        public uint Score { get; set; }

        /// <summary>
        /// Is player connected
        /// </summary>
        [JsonProperty("connected")]
        public bool IsConnected { get; set; }

        /// <summary>
        /// Player last score
        /// </summary>
        [JsonProperty("lastScore")]
        public uint LastScore { get; set; }

        /// <summary>
        /// Player rank
        /// </summary>
        [JsonProperty("rank")]
        public uint Rank { get; set; }

        /// <summary>
        /// Player state
        /// </summary>
        [JsonProperty("state")]
        public EPlayerState State { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (ID != null) &&
            (Name != null) &&
            (State != EPlayerState.Invalid);
    }
}
