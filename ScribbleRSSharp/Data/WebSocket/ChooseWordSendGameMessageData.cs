/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Choose word send game message data class
    /// </summary>
    internal class ChooseWordSendGameMessageData : GameMessageData<uint>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index">Choose word index</param>
        public ChooseWordSendGameMessageData(uint index)
        {
            Type = "choose-word";
            Data = index;
        }
    }
}
