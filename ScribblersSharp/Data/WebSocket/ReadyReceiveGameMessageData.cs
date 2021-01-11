using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "ready" receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ReadyReceiveGameMessageData : GameMessageData<ReadyData>, IReceiveGameMessageData
    {
        // ...
    }
}
