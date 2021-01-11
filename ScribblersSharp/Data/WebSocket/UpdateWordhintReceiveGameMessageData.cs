using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "update-wordhint" receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UpdateWordhintReceiveGameMessageData : GameMessageData<WordHintData[]>, IReceiveGameMessageData
    {
        // ...
    }
}
