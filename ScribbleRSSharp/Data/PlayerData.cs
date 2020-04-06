using ScribbleRSSharp.JSONConverters;
using System;
using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Player data class
    /// </summary>
    internal class PlayerData
    {
        /// <summary>
        /// Player ID
        /// </summary>
        [JsonPropertyName("id")]
        public uint ID { get; set; }

        /// <summary>
        /// Player name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Player score
        /// </summary>
        [JsonPropertyName("score")]
        public uint Score { get; set; }

        /// <summary>
        /// Is player connected
        /// </summary>
        [JsonPropertyName("connected")]
        public bool IsConnected { get; set; }

        /// <summary>
        /// Player last score
        /// </summary>
        [JsonPropertyName("lastScore")]
        public uint LastScore { get; set; }

        /// <summary>
        /// Player rank
        /// </summary>
        [JsonPropertyName("rank")]
        public uint Rank { get; set; }

        /// <summary>
        /// Player state
        /// </summary>
        [JsonPropertyName("state")]
        [JsonConverter(typeof(PlayerStateJSONConverter))]
        public EPlayerState State { get; set; }

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
        public PlayerData(uint id, string name, uint score, bool isConnected, uint lastScore, uint rank, EPlayerState state)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            ID = id;
            Name = name;
            Score = score;
            IsConnected = isConnected;
            LastScore = lastScore;
            Rank = rank;
            State = state;
        }
    }
}
