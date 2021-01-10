using Newtonsoft.Json;
/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Guessing chat message receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class MessageReceiveGameMessageData : GameMessageData<ChatMessageData>, IReceiveGameMessageData
    {
        // ...
    }
}
