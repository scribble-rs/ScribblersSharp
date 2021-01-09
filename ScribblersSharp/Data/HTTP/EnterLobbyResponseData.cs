using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Enter lobby response data class
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
        /// Is object in a valid state
        /// </summary>
        public bool IsValid => LobbyID != null;
    }
}
