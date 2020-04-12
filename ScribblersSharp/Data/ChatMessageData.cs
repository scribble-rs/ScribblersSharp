using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Chat message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChatMessageData
    {
        /// <summary>
        /// Author
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
