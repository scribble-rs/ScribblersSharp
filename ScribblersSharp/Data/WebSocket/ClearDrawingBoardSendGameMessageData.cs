using Newtonsoft.Json;
/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Clear drawing board send game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClearDrawingBoardSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ClearDrawingBoardSendGameMessageData() : base(Naming.GetSendGameMessageDataNameInKebabCase<ClearDrawingBoardSendGameMessageData>())
        {
            // ...
        }
    }
}
