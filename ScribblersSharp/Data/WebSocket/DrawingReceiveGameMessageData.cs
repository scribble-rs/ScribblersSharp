using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "drawing" receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class DrawingReceiveGameMessageData : BaseGameMessageData, IReceiveGameMessageData
    {
        // ...
    }
}
