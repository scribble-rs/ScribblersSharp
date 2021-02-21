using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a base game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseGameMessageData : IBaseGameMessageData
    {
        /// <summary>
        /// Game message type
        /// </summary>
        [JsonProperty("type")]
        public string MessageType { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public virtual bool IsValid => MessageType != null;

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
        public BaseGameMessageData(string type) => MessageType = type ?? throw new ArgumentNullException(nameof(type));
    }
}
