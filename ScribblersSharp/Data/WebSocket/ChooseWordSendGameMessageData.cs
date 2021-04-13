using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "choose-word" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChooseWordSendGameMessageData : GameMessageData<uint>, ISendGameMessageData
    {
        /// <summary>
        /// Constructs a sendable "choose-word" game message
        /// </summary>
        /// <param name="index">Choose word index</param>
        public ChooseWordSendGameMessageData(uint index) : base(Naming.GetSendGameMessageDataNameInKebabCase<ChooseWordSendGameMessageData>(), index)
        {
            // ...
        }
    }
}
