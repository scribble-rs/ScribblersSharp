using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Non-guessing chat message receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NonGuessingPlayerMessageReceiveGameMessageData : GameMessageData<ChatMessageData>, IReceiveGameMessageData
    {
        // ...
    }
}
