using Newtonsoft.Json;
/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Line draw receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LineDrawReceiveGameMessageData : GameMessageData<LineData>, IReceiveGameMessageData
    {
        // ...
    }
}
