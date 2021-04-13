using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "update-wordhint" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UpdateWordhintReceiveGameMessageData : GameMessageData<WordHintData[]>, IReceiveGameMessageData
    {
        // ...
    }
}
