using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "message" send game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class MessageSendGameMessageData : GameMessageData<string>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Content</param>
        public MessageSendGameMessageData(string content) : base(Naming.GetSendGameMessageDataNameInKebabCase<MessageSendGameMessageData>(), content)
        {
            // ...
        }
    }
}
