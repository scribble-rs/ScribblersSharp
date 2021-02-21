using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "fill" game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class FillReceiveGameMessageData : GameMessageData<FillData>, IReceiveGameMessageData
    {
        // ...
    }
}
