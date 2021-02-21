using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "clear-drawing-board" game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class DrawerKickedReceiveGameMessageData : BaseGameMessageData, IReceiveGameMessageData
    {
        // ...
    }
}
