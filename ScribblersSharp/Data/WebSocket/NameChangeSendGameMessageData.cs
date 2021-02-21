using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "name-change" game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NameChangeSendGameMessageData : GameMessageData<string>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newUsername">New username</param>
        public NameChangeSendGameMessageData(string newUsername) : base(Naming.GetSendGameMessageDataNameInKebabCase<NameChangeSendGameMessageData>(), newUsername)
        {
            // ...
        }
    }
}
