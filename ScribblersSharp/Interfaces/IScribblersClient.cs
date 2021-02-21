using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a Scribble.rs client
    /// </summary>
    public interface IScribblersClient : IDisposable
    {
        /// <summary>
        /// Scribble.rs host
        /// </summary>
        string Host { get; }

        /// <summary>
        /// Is using secure protocols
        /// </summary>
        bool IsUsingSecureProtocols { get; }

        /// <summary>
        /// Enters a lobby asynchronously
        /// </summary>
        /// <param name="lobbyID">Lobby ID</param>
        /// <param name="username">Username</param>
        /// <returns>Lobby task</returns>
        Task<ILobby> EnterLobbyAsync(string lobbyID, string username);

        /// <summary>
        /// Creates a new lobby asynchronously
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="language">Language</param>
        /// <param name="isPublic">Is lobby public</param>
        /// <param name="maximalPlayerCount">Maximal player count</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="roundCount">Round count</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="isVotekickingEnabled">Is votekicking enabled</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        /// <returns>Lobby task</returns>
        Task<ILobby> CreateLobbyAsync(string username, ELanguage language, bool isPublic, uint maximalPlayerCount, ulong drawingTime, uint roundCount, IReadOnlyList<string> customWords, uint customWordsChance, bool isVotekickingEnabled, uint clientsPerIPLimit);

        /// <summary>
        /// Gets server statistics asynchronously
        /// </summary>
        /// <returns>Server statistics task</returns>
        Task<IServerStatistics> GetServerStatisticsAsync();

        /// <summary>
        /// Lists all public lobbies asynchronously
        /// </summary>
        /// <returns>Lobby views task</returns>
        Task<IEnumerable<ILobbyView>> ListLobbiesAsync();

        /// <summary>
        /// Changes lobby rules asynchronously
        /// </summary>
        /// <param name="language">Language (optional)</param>
        /// <param name="isPublic">Is lobby public (optional)</param>
        /// <param name="maximalPlayerCount">Maximal player count (optional)</param>
        /// <param name="drawingTime">Drawing time (optional)</param>
        /// <param name="roundCount">Round count (optional)</param>
        /// <param name="customWords">Custom words (optional)</param>
        /// <param name="customWordsChance">Custom words chance (optional)</param>
        /// <param name="isVotekickingEnabled">Is votekicking enabled (optional)</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit (optional)</param>
        /// <returns>Task</returns>
        Task ChangeLobbyRulesAsync(ELanguage? language = null, bool? isPublic = null, uint? maximalPlayerCount = null, ulong? drawingTime = null, uint? roundCount = null, IReadOnlyList<string> customWords = null, uint? customWordsChance = null, bool? isVotekickingEnabled = null, uint? clientsPerIPLimit = null);
    }
}
