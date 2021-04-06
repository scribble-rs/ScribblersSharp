/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A structure that describes server statistics
    /// </summary>
    public readonly struct ServerStatistics : IServerStatistics
    {
        /// <summary>
        /// Is connection secure
        /// </summary>
        public bool IsConnectionSecure { get; }

        /// <summary>
        /// Active lobby count
        /// </summary>
        public uint ActiveLobbyCount { get; }

        /// <summary>
        /// Player count
        /// </summary>
        public uint PlayerCount { get; }

        /// <summary>
        /// Occupied player slot count
        /// </summary>
        public uint OccupiedPlayerSlotCount { get; }

        /// <summary>
        /// Connected player count
        /// </summary>
        public uint ConnectedPlayerCount { get; }

        /// <summary>
        /// Constructs server statistics
        /// </summary>
        /// <param name="isConnectionSecure">Is connection secure</param>
        /// <param name="activeLobbyCount">Active lobby count</param>
        /// <param name="playerCount">Player count</param>
        /// <param name="occupiedPlayerSlotCount">Occupied player slot count</param>
        /// <param name="connectedPlayerCount">Connected player count</param>
        public ServerStatistics(bool isConnectionSecure, uint activeLobbyCount, uint playerCount, uint occupiedPlayerSlotCount, uint connectedPlayerCount)
        {
            IsConnectionSecure = isConnectionSecure;
            ActiveLobbyCount = activeLobbyCount;
            PlayerCount = playerCount;
            OccupiedPlayerSlotCount = occupiedPlayerSlotCount;
            ConnectedPlayerCount = connectedPlayerCount;
        }
    }
}
