using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Base game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseGameMessageData : IBaseGameMessageData
    {
        /// <summary>
        /// Game message type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public virtual bool IsValid => Type != null;

        /// <summary>
        /// Constructs game message data for serializers
        /// </summary>
        public BaseGameMessageData()
        {
            // ...
        }

        /// <summary>
        /// Constructs game message data
        /// </summary>
        /// <param name="data">Message data</param>
        public BaseGameMessageData(string type) => Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}
