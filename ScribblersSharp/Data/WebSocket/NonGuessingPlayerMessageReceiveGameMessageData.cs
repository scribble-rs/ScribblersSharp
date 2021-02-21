using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "non-guessing-player-message" game message .
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NonGuessingPlayerMessageReceiveGameMessageData : GameMessageData<ChatMessageData>, IReceiveGameMessageData
    {
        // ...
    }
}
