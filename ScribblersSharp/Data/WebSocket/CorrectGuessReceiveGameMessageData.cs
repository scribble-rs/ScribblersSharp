using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Correct guess receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class CorrectGuessReceiveGameMessageData : GameMessageData<string>, IReceiveGameMessageData
    {
        // ...
    }
}
