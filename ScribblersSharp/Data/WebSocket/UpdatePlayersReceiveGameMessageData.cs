using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "update-players" game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UpdatePlayersReceiveGameMessageData : GameMessageData<PlayerData[]>, IReceiveGameMessageData
    {
        // ...
    }
}
