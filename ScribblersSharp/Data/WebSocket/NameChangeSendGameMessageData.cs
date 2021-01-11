using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "name-change" send game message data class
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
