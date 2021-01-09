using Newtonsoft.Json;
using System.Drawing;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Fill draw send game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class FillDrawSendGameMessageData : GameMessageData<FillData>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Fill X</param>
        /// <param name="y">Fill Y</param>
        /// <param name="color">Fill color</param>
        public FillDrawSendGameMessageData(float x, float y, Color color) : base("fill", new FillData(x, y, color))
        {
            // ...
        }
    }
}
