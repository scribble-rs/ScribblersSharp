using Newtonsoft.Json;
using ScribblersSharp.JSONConverters;
using System.Drawing;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Fill data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class FillData
    {
        /// <summary>
        /// Fill X
        /// </summary>
        [JsonProperty("x")]
        public float X { get; set; }

        /// <summary>
        /// Fill Y
        /// </summary>
        [JsonProperty("y")]
        public float Y { get; set; }

        /// <summary>
        /// Fill color
        /// </summary>
        [JsonProperty("color")]
        [JsonConverter(typeof(ColorJSONConverter))]
        public Color Color { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FillData()
        {
            // ...
        }

        /// <summary>
        /// Constructor
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
