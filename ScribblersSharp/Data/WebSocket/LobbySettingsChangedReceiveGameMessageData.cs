using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "lobby-settings-changed" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LobbySettingsChangedReceiveGameMessageData : GameMessageData<LobbySettingsChangeData>, IReceiveGameMessageData
    {
        // ...
    }
}
