using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "system-message" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class SystemMessageReceiveGameMessageData : GameMessageData<string>, IReceiveGameMessageData
    {
        // ...
    }
}
