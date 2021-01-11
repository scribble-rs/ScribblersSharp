using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "update-players" receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UpdatePlayersReceiveGameMessageData : GameMessageData<PlayerData[]>, IReceiveGameMessageData
    {
        // ...
    }
}
