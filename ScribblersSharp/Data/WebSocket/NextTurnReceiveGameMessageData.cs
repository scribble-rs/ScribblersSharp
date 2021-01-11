using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "next-turn" receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class NextTurnReceiveGameMessageData : GameMessageData<NextTurnData>, IReceiveGameMessageData
    {
        // ...
    }
}
