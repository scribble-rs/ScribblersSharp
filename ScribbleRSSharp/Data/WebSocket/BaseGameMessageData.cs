using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
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
