using Newtonsoft.Json;
/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Fill draw receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class FillReceiveGameMessageData : GameMessageData<FillData>, IReceiveGameMessageData
    {
        // ...
    }
}
