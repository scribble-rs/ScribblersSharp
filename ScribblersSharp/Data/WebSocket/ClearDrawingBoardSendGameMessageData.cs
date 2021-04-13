using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "clear-drawing-board" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClearDrawingBoardSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Constructs a sendable "clear-drawing-board" game message
        /// </summary>
        public ClearDrawingBoardSendGameMessageData() : base(Naming.GetSendGameMessageDataNameInKebabCase<ClearDrawingBoardSendGameMessageData>())
        {
            // ...
        }
    }
}
