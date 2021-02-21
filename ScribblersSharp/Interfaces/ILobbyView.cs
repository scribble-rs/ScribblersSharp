/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a lobby view
    /// </summary>
    public interface ILobbyView : IValidable
    {
        /// <summary>
        /// Lobby ID
        /// </summary>
        string LobbyID { get; }

        /// <summary>
        /// Player count
        /// </summary>
        uint PlayerCount { get; }

        /// <summary>
        /// Maximal player count
        /// </summary>
        uint MaximalPlayerCount { get; }

        /// <summary>
        /// Round count
        /// </summary>
        uint RoundCount { get; }

        /// <summary>
        /// Maximal round count
        /// </summary>
        uint MaximalRoundCount { get; }

        /// <summary>
        /// Drawing time
        /// </summary>
        uint DrawingTime { get; }

        /// <summary>
        /// Is using custom words
        /// </summary>
        bool IsUsingCustomWords { get; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        bool IsVotekickingEnabled { get; }

        /// <summary>
        /// Maximal clients per IP count
        /// </summary>
        uint MaximalClientsPerIPCount { get; }

        /// <summary>
        /// Language
        /// </summary>
        ELanguage Language { get; }
    }
}
