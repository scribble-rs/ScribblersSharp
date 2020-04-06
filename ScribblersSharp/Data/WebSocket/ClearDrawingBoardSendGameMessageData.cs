/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Clear drawing board send game message data class
    /// </summary>
    internal class ClearDrawingBoardSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ClearDrawingBoardSendGameMessageData()
        {
            Type = "clear-drawing-board";
        }
    }
}
