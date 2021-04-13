using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "owner-change" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class OwnerChangeReceiveGameMessageData : GameMessageData<OwnerChangeData>, IReceiveGameMessageData
    {
        // ...
    }
}
