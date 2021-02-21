using Newtonsoft.Json;
using System.Drawing;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "fill" game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class FillSendGameMessageData : GameMessageData<FillData>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Fill X</param>
        /// <param name="y">Fill Y</param>
        /// <param name="color">Fill color</param>
        public FillSendGameMessageData(float x, float y, Color color) : base(Naming.GetSendGameMessageDataNameInKebabCase<FillSendGameMessageData>(), new FillData(x, y, color))
        {
            // ...
        }
    }
}
