using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "choose-word" send game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChooseWordSendGameMessageData : GameMessageData<uint>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index">Choose word index</param>
        public ChooseWordSendGameMessageData(uint index) : base(Naming.GetSendGameMessageDataNameInKebabCase<ChooseWordSendGameMessageData>(), index)
        {
            // ...
        }
    }
}
