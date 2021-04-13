/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A structure that describes a color
    /// </summary>
    public readonly struct Color : IColor
    {
        /// <summary>
        /// Red color component
        /// </summary>
        public byte Red { get; }

        /// <summary>
        /// Green color component
        /// </summary>
        public byte Green { get; }

        /// <summary>
        /// Blue color component
        /// </summary>
        public byte Blue { get; }

        /// <summary>
        /// Constructs a color
        /// </summary>
        /// <param name="red">Red color component</param>
        /// <param name="green">Green color component</param>
        /// <param name="blue">Blue color component</param>
        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
