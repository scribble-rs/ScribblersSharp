using System.Text.Json.Serialization;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Base game message data class
    /// </summary>
    public class BaseGameMessageData : IGameMessageData
    {
        /// <summary>
        /// Game message type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
