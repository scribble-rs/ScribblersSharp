using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "line" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LineReceiveGameMessageData : GameMessageData<LineData>, IReceiveGameMessageData
    {
        // ...
    }
}
