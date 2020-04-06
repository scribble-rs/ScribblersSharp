using System.Text.Json.Serialization;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Chat message data class
    /// </summary>
    internal class ChatMessageData
    {
        /// <summary>
        /// Author
        /// </summary>
        [JsonPropertyName("author")]
        public string Author { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
