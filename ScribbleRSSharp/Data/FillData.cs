using ScribbleRSSharp.JSONConverters;
using System.Drawing;
using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Fill data class
    /// </summary>
    internal class FillData
    {
        /// <summary>
        /// Fill X
        /// </summary>
        [JsonPropertyName("x")]
        public float X { get; set; }

        /// <summary>
        /// Fill Y
        /// </summary>
        [JsonPropertyName("y")]
        public float Y { get; set; }

        /// <summary>
        /// Fill color
        /// </summary>
        [JsonPropertyName("color")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color Color { get; set; }

        /// <summary>
        /// COnstructor
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="color">Color</param>
        public FillData(float x, float y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
}
