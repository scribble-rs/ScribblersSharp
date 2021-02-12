using Newtonsoft.Json;
using ScribblersSharp.JSONConverters;
using System.Drawing;

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
        public byte[] CanvasColor { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (LobbyID != null) &&
            (DrawingBoardBaseWidth > 0U) &&
            (DrawingBoardBaseHeight > 0U) &&
            (MinimalBrushSize > 0U) &&
            (MaximalBrushSize >= MinimalBrushSize) &&
            (SuggestedBrushSizes != null) &&
            (CanvasColor != null) &&
            (CanvasColor.Length == 3);
    }
}
