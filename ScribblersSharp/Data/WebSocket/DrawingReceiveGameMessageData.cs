using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "drawing" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class DrawingReceiveGameMessageData : BaseGameMessageData, IReceiveGameMessageData
    {
        // ...
    }
}
