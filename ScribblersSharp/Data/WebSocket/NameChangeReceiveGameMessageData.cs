using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "name-change" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NameChangeReceiveGameMessageData : GameMessageData<NameChangeData>, IReceiveGameMessageData
    {
        // ...
    }
}
