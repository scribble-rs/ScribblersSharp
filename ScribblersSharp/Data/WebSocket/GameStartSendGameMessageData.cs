/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Game start send game message data class
    /// </summary>
    internal class GameStartSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GameStartSendGameMessageData()
        {
            Type = "start";
        }
    }
}
