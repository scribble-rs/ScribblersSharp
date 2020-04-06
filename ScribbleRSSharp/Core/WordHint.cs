/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Word hint structure
    /// </summary>
    public struct WordHint
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
        internal WordHint(char character, bool underline)
        {
            Character = character;
            Underline = underline;
        }
    }
}
