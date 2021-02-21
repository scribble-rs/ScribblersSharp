using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "message" game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class MessageReceiveGameMessageData : GameMessageData<ChatMessageData>, IReceiveGameMessageData
    {
        // ...
    }
}
