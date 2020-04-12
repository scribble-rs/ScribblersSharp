using Newtonsoft.Json;
/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Choose word receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChooseWordReceiveGameMessageData : GameMessageData<int>, IReceiveGameMessageData
    {
        // ...
    }
}
