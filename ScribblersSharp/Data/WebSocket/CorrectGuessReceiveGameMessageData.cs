using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "correct-guess" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class CorrectGuessReceiveGameMessageData : GameMessageData<string>, IReceiveGameMessageData
    {
        // ...
    }
}
