using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "message" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class MessageSendGameMessageData : GameMessageData<string>, ISendGameMessageData
    {
        /// <summary>
        /// Constructs a sendable "message" game message
        /// </summary>
        /// <param name="content">Content</param>
        public MessageSendGameMessageData(string content) : base(Naming.GetSendGameMessageDataNameInKebabCase<MessageSendGameMessageData>(), content)
        {
            // ...
        }
    }
}
