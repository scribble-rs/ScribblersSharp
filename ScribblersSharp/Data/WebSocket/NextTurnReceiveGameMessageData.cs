using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "next-turn" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NextTurnReceiveGameMessageData : GameMessageData<NextTurnData>, IReceiveGameMessageData
    {
        // ...
    }
}
