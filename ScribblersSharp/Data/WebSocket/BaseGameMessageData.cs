using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Base game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseGameMessageData : IGameMessageData
    {
        /// <summary>
        /// Game message type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
