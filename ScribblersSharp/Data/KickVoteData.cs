using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes kick vote data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class KickVoteData : IValidable
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
        /// Vote count
        /// </summary>
        [JsonProperty("voteCount")]
        public uint VoteCount { get; set; }

        /// <summary>
        /// Vote count
        /// </summary>
        [JsonProperty("requiredVoteCount")]
        public uint RequiredVoteCount { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(PlayerID) &&
            !string.IsNullOrWhiteSpace(PlayerName) &&
            (VoteCount >= 0U) &&
            (RequiredVoteCount >= 0U);
    }
}
