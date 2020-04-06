using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Word hint data class
    /// </summary>
    internal class WordHintData
    {
        /// <summary>
        /// Character
        /// </summary>
        [JsonPropertyName("character")]
        public char Character { get; set; }

        /// <summary>
        /// Underline
        /// </summary>
        [JsonPropertyName("underline")]
        public bool Underline { get; set; }

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
