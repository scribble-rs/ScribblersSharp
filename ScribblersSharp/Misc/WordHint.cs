/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Word hint structure
    /// </summary>
    internal readonly struct WordHint : IWordHint
    {
        /// <summary>
        /// Character
        /// </summary>
        public char Character { get; }

        /// <summary>
        /// Underline
        /// </summary>
        public bool Underline { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="character">Character</param>
        /// <param name="underline">Underline</param>
        public WordHint(char character, bool underline)
        {
            Character = character;
            Underline = underline;
        }
    }
}
