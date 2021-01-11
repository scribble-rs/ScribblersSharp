using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "line" receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LineReceiveGameMessageData : GameMessageData<LineData>, IReceiveGameMessageData
    {
        // ...
    }
}
