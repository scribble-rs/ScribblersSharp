using System.Drawing;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Fill draw send game message data class
    /// </summary>
    internal class FillDrawSendGameMessageData : GameMessageData<FillData>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Fill X</param>
        /// <param name="y">Fill Y</param>
        /// <param name="color">Fill color</param>
        public FillDrawSendGameMessageData(float x, float y, Color color)
        {
            Type = "fill";
            Data = new FillData(x, y, color);
        }
    }
}
