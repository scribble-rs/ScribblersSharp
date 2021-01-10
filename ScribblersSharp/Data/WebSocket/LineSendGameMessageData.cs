using Newtonsoft.Json;
using System.Drawing;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Line send game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LineSendGameMessageData : GameMessageData<LineData>, ISendGameMessageData
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
        public LineSendGameMessageData(float fromX, float fromY, float toX, float toY, Color color, float lineWidth) : base(Naming.GetSendGameMessageDataNameInKebabCase<LineSendGameMessageData>(), new LineData(fromX, fromY, toX, toY, color, lineWidth))
        {
            // ...
        }
    }
}
