using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes response data for entering a lobby
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class EnterLobbyResponseData : IResponseData
    {
        /// <summary>
        /// Lobby ID
        /// </summary>
        [JsonProperty("lobbyId")]
        public string LobbyID { get; set; }

        /// <summary>
        /// Minimal drawing time in seconds
        /// </summary>
        [JsonProperty("minDrawingTime")]
        public uint MinimalDrawingTime { get; set; }

        /// <summary>
        /// Maximal drawing time in seconds
        /// </summary>
        [JsonProperty("maxDrawingTime")]
        public uint MaximalDrawingTime { get; set; }

        /// <summary>
        /// Minimal round count
        /// </summary>
        [JsonProperty("minRounds")]
        public uint MinimalRoundCount { get; set; }

        /// <summary>
        /// Maximal round count
        /// </summary>
        [JsonProperty("maxRounds")]
        public uint MaximalRoundCount { get; set; }

        /// <summary>
        /// Minimal of maximal player count
        /// </summary>
        [JsonProperty("minMaxPlayers")]
        public uint MinimalMaximalPlayerCount { get; set; }

        /// <summary>
        /// Maximal of maximal player count
        /// </summary>
        [JsonProperty("maxMaxPlayers")]
        public uint MaximalMaximalPlayerCount { get; set; }

        /// <summary>
        /// Minimal clients per IP count limit
        /// </summary>
        [JsonProperty("minClientsPerIpLimit")]
        public uint MinimalClientsPerIPLimit { get; set; }

        /// <summary>
        /// Maximal clients per IP count limit
        /// </summary>
        [JsonProperty("maxClientsPerIpLimit")]
        public uint MaximalClientsPerIPLimit { get; set; }

        /// <summary>
        /// Maximal player count
        /// </summary>
        [JsonProperty("maxPlayers")]
        public uint MaximalPlayerCount { get; set; }

        /// <summary>
        /// Current maximal round count
        /// </summary>
        [JsonProperty("rounds")]
        public uint CurrentMaximalRoundCount { get; set; }

        /// <summary>
        /// Is lobby public
        /// </summary>
        [JsonProperty("public")]
        public bool IsLobbyPublic { get; set; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        [JsonProperty("enableVotekick")]
        public bool IsVotekickingEnabled { get; set; }

        /// <summary>
        /// Custom words chance
        /// </summary>
        [JsonProperty("customWordsChance")]
        public uint CustomWordsChance { get; set; }

        /// <summary>
        /// Allowed clients per IP count
        /// </summary>
        [JsonProperty("clientsPerIpLimit")]
        public uint AllowedClientsPerIPCount { get; set; }

        /// <summary>
        /// Drawing board base width
        /// </summary>
        [JsonProperty("drawingBoardBaseWidth")]
        public uint DrawingBoardBaseWidth { get; set; }

        /// <summary>
        /// Drawing board base height
        /// </summary>
        [JsonProperty("drawingBoardBaseHeight")]
        public uint DrawingBoardBaseHeight { get; set; }

        /// <summary>
        /// Minimal brush size
        /// </summary>
        [JsonProperty("minBrushSize")]
        public uint MinimalBrushSize { get; set; }

        /// <summary>
        /// Maximal brush size
        /// </summary>
        [JsonProperty("maxBrushSize")]
        public uint MaximalBrushSize { get; set; }

        /// <summary>
        /// Suggested brush size
        /// </summary>
        [JsonProperty("suggestedBrushSizes")]
        public uint[] SuggestedBrushSizes { get; set; }

        /// <summary>
        /// Canvas color
        /// </summary>
        [JsonProperty("canvasColor")]
        public ColorData CanvasColor { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (LobbyID != null) &&
            (MinimalDrawingTime > 0U) &&
            (MaximalDrawingTime >= MinimalDrawingTime) &&
            (MinimalRoundCount > 0U) &&
            (MaximalRoundCount >= MinimalRoundCount) &&
            (MinimalMaximalPlayerCount > 0U) &&
            (MaximalMaximalPlayerCount >= MinimalMaximalPlayerCount) &&
            (MinimalClientsPerIPLimit > 0U) &&
            (MaximalClientsPerIPLimit >= MinimalClientsPerIPLimit) &&
            (MaximalPlayerCount >= MinimalMaximalPlayerCount) &&
            (MaximalPlayerCount <= MaximalMaximalPlayerCount) &&
            (CurrentMaximalRoundCount >= MinimalRoundCount) &&
            (CurrentMaximalRoundCount <= MaximalRoundCount) &&
            (AllowedClientsPerIPCount >= MinimalClientsPerIPLimit) &&
            (AllowedClientsPerIPCount <= MaximalClientsPerIPLimit) &&
            (DrawingBoardBaseWidth > 0U) &&
            (DrawingBoardBaseHeight > 0U) &&
            (MinimalBrushSize > 0U) &&
            (MaximalBrushSize >= MinimalBrushSize) &&
            (SuggestedBrushSizes != null) &&
            (CanvasColor != null);
    }
}
