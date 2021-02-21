/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A structure that describes a lobby view
    /// </summary>
    public readonly struct LobbyView : ILobbyView
    {
        /// <summary>
        /// Lobby ID
        /// </summary>
        public string LobbyID { get; }

        /// <summary>
        /// Player count
        /// </summary>
        public uint PlayerCount { get; }

        /// <summary>
        /// Maximal player count
        /// </summary>
        public uint MaximalPlayerCount { get; }

        /// <summary>
        /// Round count
        /// </summary>
        public uint RoundCount { get; }

        /// <summary>
        /// Maximal round count
        /// </summary>
        public uint MaximalRoundCount { get; }

        /// <summary>
        /// Drawing time
        /// </summary>
        public uint DrawingTime { get; }

        /// <summary>
        /// Is using custom words
        /// </summary>
        public bool IsUsingCustomWords { get; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        public bool IsVotekickingEnabled { get; }

        /// <summary>
        /// Maximal clients per IP count
        /// </summary>
        public uint MaximalClientsPerIPCount { get; }

        /// <summary>
        /// Language
        /// </summary>
        public ELanguage Language { get; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(LobbyID) &&
            (MaximalPlayerCount > 1U) &&
            (MaximalRoundCount > 0U) &&
            (MaximalClientsPerIPCount > 0U);

        /// <summary>
        /// Constructs a lobby view
        /// </summary>
        /// <param name="lobbyID">Lobby ID</param>
        /// <param name="playerCount">Player count</param>
        /// <param name="maximalPlayerCount">Maximal player count</param>
        /// <param name="roundCount">Round count</param>
        /// <param name="maximalRoundCount">Maximal round count</param>
        /// <param name="drawingTime">Drawing time in seconds</param>
        /// <param name="isUsingCustomWords">Is using custom words</param>
        /// <param name="isVotekickingEnabled">Is votekicking enabled</param>
        /// <param name="maximalClientsPerIPCount">Maximal client per IP count</param>
        /// <param name="language">Language</param>
        public LobbyView(string lobbyID, uint playerCount, uint maximalPlayerCount, uint roundCount, uint maximalRoundCount, uint drawingTime, bool isUsingCustomWords, bool isVotekickingEnabled, uint maximalClientsPerIPCount, ELanguage language)
        {
            LobbyID = lobbyID;
            PlayerCount = playerCount;
            MaximalPlayerCount = maximalPlayerCount;
            RoundCount = roundCount;
            MaximalRoundCount = maximalRoundCount;
            DrawingTime = drawingTime;
            IsUsingCustomWords = isUsingCustomWords;
            IsVotekickingEnabled = isVotekickingEnabled;
            MaximalClientsPerIPCount = maximalClientsPerIPCount;
            Language = language;
        }
    }
}
