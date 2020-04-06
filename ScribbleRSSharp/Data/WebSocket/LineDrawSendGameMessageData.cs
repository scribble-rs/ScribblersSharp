using System.Drawing;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Line send game message data class
    /// </summary>
    internal class LineDrawSendGameMessageData : GameMessageData<LineData>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fromX">Line from X</param>
        /// <param name="fromY">Line from Y</param>
        /// <param name="toX">Line to X</param>
        /// <param name="toY">Line to Y</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width</param>
        public LineDrawSendGameMessageData(float fromX, float fromY, float toX, float toY, Color color, float lineWidth)
        {
            Type = "line";
            Data = new LineData(fromX, fromY, toX, toY, color, lineWidth);
        }
    }
}
