/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents server statistics
    /// </summary>
    public interface IServerStatistics
    {
        /// <summary>
        /// Is connection secure
        /// </summary>
        bool IsConnectionSecure { get; }

        /// <summary>
        /// Active lobby count
        /// </summary>
        uint ActiveLobbyCount { get; }

        /// <summary>
        /// Player count
        /// </summary>
        uint PlayerCount { get; }

        /// <summary>
        /// Occupied player slot count
        /// </summary>
        uint OccupiedPlayerSlotCount { get; }

        /// <summary>
        /// Connected player count
        /// </summary>
        uint ConnectedPlayerCount { get; }
    }
}
