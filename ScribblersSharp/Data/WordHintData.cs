using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Word hint data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class WordHintData
    {
        /// <summary>
        /// Character
        /// </summary>
        [JsonProperty("character")]
        public char Character { get; set; }

        /// <summary>
        /// Underline
        /// </summary>
        [JsonProperty("underline")]
        public bool Underline { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WordHintData()
        {
            // ...
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="character">Character</param>
        /// <param name="underline">Underline</param>
        public WordHintData(char character, bool underline)
        {
            Character = character;
            Underline = underline;
        }
    }
}
