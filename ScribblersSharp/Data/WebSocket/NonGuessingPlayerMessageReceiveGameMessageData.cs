using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "non-guessing-player-message" receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NonGuessingPlayerMessageReceiveGameMessageData : GameMessageData<ChatMessageData>, IReceiveGameMessageData
    {
        // ...
    }
}
