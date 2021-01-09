using Newtonsoft.Json;
using ScribblersSharp.JSONConverters;
using System;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Player data class
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
        [JsonConverter(typeof(PlayerStateJSONConverter))]
        public EPlayerState State { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (ID != null) &&
            (Name != null) &&
            (State != EPlayerState.Unknown);

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlayerData()
        {
            // ...
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="name">Player name</param>
        /// <param name="score">Player score</param>
        /// <param name="isConnected">Is player connected</param>
        /// <param name="lastScore">Last player score</param>
        /// <param name="rank">Player rank</param>
        /// <param name="state">Player state</param>
        public PlayerData(string id, string name, uint score, bool isConnected, uint lastScore, uint rank, EPlayerState state)
        {
            if (state == EPlayerState.Unknown)
            {
                throw new ArgumentException("Player state is unknown.", nameof(state));
            }
            ID = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Score = score;
            IsConnected = isConnected;
            LastScore = lastScore;
            Rank = rank;
            State = state;
        }
    }
}
