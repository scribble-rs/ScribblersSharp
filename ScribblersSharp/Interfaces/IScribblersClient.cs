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
        /// Enters a lobby (asynchronous)
        /// </summary>
        /// <param name="lobbyID">Lobby ID</param>
        /// <param name="username">Username</param>
        /// <returns>Lobby task</returns>
        Task<ILobby> EnterLobbyAsync(string lobbyID, string username);

        /// <summary>
        /// Creates a new lobby (asynchronous)
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="language">Language</param>
        /// <param name="isPublic">Is lobby public</param>
        /// <param name="maximalPlayers">Maximal players</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="rounds">Rounds</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="enableVotekick">Enable vote kick</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        /// <returns>Lobby task</returns>
        Task<ILobby> CreateLobbyAsync(string username, ELanguage language, bool isPublic, uint maximalPlayers, ulong drawingTime, uint rounds, IReadOnlyList<string> customWords, uint customWordsChance, bool enableVotekick, uint clientsPerIPLimit);

        /// <summary>
        /// Lists all public lobbies asynchronously
        /// </summary>
        /// <returns>Lobbies</returns>
        Task<IEnumerable<ILobbyView>> ListLobbies();
    }
}
