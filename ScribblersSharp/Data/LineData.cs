using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Line data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LineData : IValidable
    {
        /// <summary>
        /// Line from X
        /// </summary>
        [JsonProperty("fromX")]
        public float FromX { get; set; }

        /// <summary>
        /// Line from Y
        /// </summary>
        [JsonProperty("fromY")]
        public float FromY { get; set; }

        /// <summary>
        /// Line to X
        /// </summary>
        [JsonProperty("toX")]
        public float ToX { get; set; }

        /// <summary>
        /// Line to Y
        /// </summary>
        [JsonProperty("toY")]
        public float ToY { get; set; }

        /// <summary>
        /// Line color
        /// </summary>
        [JsonProperty("color")]
        public ColorData Color { get; set; }

        /// <summary>
        /// Line width
        /// </summary>
        [JsonProperty("lineWidth")]
        public float LineWidth { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (Color != null) &&
            (LineWidth > float.Epsilon);

        /// <summary>
        /// Constructs line data for deserializers
        /// </summary>
        public LineData()
        {
            // ...
        }

        /// <summary>
        /// Constructs line data
        /// </summary>
        /// <param name="fromX">Line from X</param>
        /// <param name="fromY">Line from Y</param>
        /// <param name="toX">Line to X</param>
        /// <param name="toY">Line to Y</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width</param>
        public LineData(float fromX, float fromY, float toX, float toY, Color color, float lineWidth)
        {
            FromX = fromX;
            FromY = fromY;
            ToX = toX;
            ToY = toY;
            Color = (ColorData)color;
            LineWidth = lineWidth;
        }
    }
}
