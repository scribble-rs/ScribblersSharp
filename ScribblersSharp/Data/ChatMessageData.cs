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
    internal class ChatMessageData : IValidable
    {
        /// <summary>
        /// Author
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// Author ID
        /// </summary>
        [JsonProperty("authorID")]
        public string AuthorID { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (Author != null) &&
            (AuthorID != null) &&
            (Content != null);
    }
}
