using Newtonsoft.Json;
using ScribblersSharp.JSONConverters;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes word hint data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class WordHintData : IValidable
    {
        /// <summary>
        /// Character
        /// </summary>
        [JsonProperty("character")]
        [JsonConverter(typeof(CharacterJSONConverter))]
        public char Character { get; set; }

        /// <summary>
        /// Underline
        /// </summary>
        [JsonProperty("underline")]
        public bool Underline { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid => true;
    }
}
