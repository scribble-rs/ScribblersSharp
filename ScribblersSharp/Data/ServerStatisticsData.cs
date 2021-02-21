using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes server statistics data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ServerStatisticsData
    {
        /// <summary>
        /// Active lobby count
        /// </summary>
        [JsonProperty("activeLobbyCount")]
        public uint ActiveLobbyCount { get; set; }

        /// <summary>
        /// Player count
        /// </summary>
        [JsonProperty("playersCount")]
        public uint PlayerCount { get; set; }

        /// <summary>
        /// Occupied player slot count
        /// </summary>
        [JsonProperty("occupiedPlayerSlotCount")]
        public uint OccupiedPlayerSlotCount { get; set; }

        /// <summary>
        /// Connected player count
        /// </summary>
        [JsonProperty("connectedPlayersCount")]
        public uint ConnectedPlayerCount { get; set; }
    }
}
