using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Create lobby request data class
    /// </summary>
    internal class CreateLobbyRequestData : IRequestData<CreateLobbyResponseData>
    {
        /// <summary>
        /// Username
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; }

        /// <summary>
        /// Language
        /// </summary>
        [JsonPropertyName("language")]
        public ELanguage Language { get; }

        /// <summary>
        /// Maximal players
        /// </summary>
        [JsonPropertyName("maxPlayers")]
        public uint MaximalPlayers { get; }

        /// <summary>
        /// Drawing time
        /// </summary>
        [JsonPropertyName("drawingTime")]
        public ulong DrawingTime { get; }

        /// <summary>
        /// Rounds
        /// </summary>
        [JsonPropertyName("rounds")]
        public uint Rounds { get; }

        /// <summary>
        /// Custom words
        /// </summary>
        [JsonPropertyName("customWords")]
        public string[] CustomWords { get; }

        /// <summary>
        /// Custom words chance
        /// </summary>
        [JsonPropertyName("customWordsChance")]
        public uint CustomWordsChance { get; }

        /// <summary>
        /// Enable votekick
        /// </summary>
        [JsonPropertyName("enableVotekick")]
        public bool EnableVotekick { get; }

        /// <summary>
        /// CLients per IP limit
        /// </summary>
        [JsonPropertyName("clientsPerIPLimit")]
        public uint ClientsPerIPLimit { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="language">Language</param>
        /// <param name="maximalPlayers">Maximal pklayers</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="rounds">Rounds</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="enableVotekick">Enable votekick</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        public CreateLobbyRequestData(string username, ELanguage language, uint maximalPlayers, ulong drawingTime, uint rounds, string[] customWords, uint customWordsChance, bool enableVotekick, uint clientsPerIPLimit)
        {
            Username = username;
            Language = language;
            MaximalPlayers = maximalPlayers;
            DrawingTime = drawingTime;
            Rounds = rounds;
            CustomWords = customWords;
            CustomWordsChance = customWordsChance;
            EnableVotekick = enableVotekick;
            ClientsPerIPLimit = clientsPerIPLimit;
        }
    }
}
