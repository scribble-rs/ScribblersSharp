using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes color data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ColorData : IValidable
    {
        /// <summary>
        /// Red color component
        /// </summary>
        [JsonProperty("r")]
        public byte Red { get; set; }

        /// <summary>
        /// Green color component
        /// </summary>
        [JsonProperty("g")]
        public byte Green { get; set; }

        /// <summary>
        /// Blue color component
        /// </summary>
        [JsonProperty("b")]
        public byte Blue { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid => true;

        /// <summary>
        /// Constructs color data for deserializers
        /// </summary>
        public ColorData()
        {
            // ...
        }

        /// <summary>
        /// Constructs color data
        /// </summary>
        /// <param name="red">Red color component</param>
        /// <param name="green">Green color component</param>
        /// <param name="blue">Blue color component</param>
        public ColorData(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        /// Explicit "ColorData" cast operator
        /// </summary>
        /// <param name="color">Color</param>
        public static explicit operator ColorData(Color color) => new ColorData(color.Red, color.Green, color.Blue);

        /// <summary>
        /// Explicit "Color" cast operator
        /// </summary>
        /// <param name="color">Color</param>
        public static explicit operator Color(ColorData color)
        {
            if (color == null)
            {
                throw new ArgumentNullException(nameof(color));
            }
            return new Color(color.Red, color.Green, color.Blue);
        }
    }
}
