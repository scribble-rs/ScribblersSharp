using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// System message receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class SystemMessageReceiveGameMessageData : GameMessageData<string>, IReceiveGameMessageData
    {
        // ...
    }
}
