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
        /// Is player allowed to draw
        /// </summary>
        [JsonProperty("allowDrawing")]
        public bool IsPlayerAllowedToDraw { get; set; }

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
        /// Game state
        /// </summary>
        [JsonProperty("gameState")]
        [JsonConverter(typeof(GameStateJSONConverter))]
        public EGameState GameState { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid
        {
            get
            {
                bool b1 = (PlayerID != null);
                bool b2 = (OwnerID != null);
                bool b3 = (GameState != EGameState.Unknown);
                bool b4 = Protection.IsValid(Players);
                bool b5 = Protection.IsContained(Players, (player) => player.ID == PlayerID);
                bool b6 = Protection.IsContained(Players, (player) => player.ID == OwnerID);
                bool b7 = Protection.AreUnique(Players, (left, right) => left.ID != right.ID);
                return b1 && b2 && b3 && b4 && b5 && b6 && b7;
            }
        }
    }
}
