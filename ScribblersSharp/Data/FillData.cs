using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes fill data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class FillData : IValidable
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
        public ColorData Color { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid => Color != null;

        /// <summary>
        /// Constructs fill data for deserializers
        /// </summary>
        public FillData()
        {
            // ...
        }

        /// <summary>
        /// Constructs fill data
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="color">Color</param>
        public FillData(float x, float y, Color color)
        {
            X = x;
            Y = y;
            Color = (ColorData)color;
        }
    }
}
