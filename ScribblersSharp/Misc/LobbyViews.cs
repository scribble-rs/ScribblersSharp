using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A class that describes lobby views
    /// </summary>
    internal class LobbyViews : List<ILobbyView>, ILobbyViews
    {
        /// <summary>
        /// Is connection secure
        /// </summary>
        public bool IsConnectionSecure { get; }

        /// <summary>
        /// Constructs lobby views
        /// </summary>
        /// <param name="isConnectionSecure">Is connection secure</param>
        /// <param name="lobbyViews">Lobby views</param>
        public LobbyViews(bool isConnectionSecure, IEnumerable<ILobbyView> lobbyViews) : base(lobbyViews) => IsConnectionSecure = isConnectionSecure;
    }
}
