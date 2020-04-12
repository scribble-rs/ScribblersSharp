using Newtonsoft.Json;
/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Clear drawing board receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClearDrawingBoardReceiveGameMessageData : BaseGameMessageData, IReceiveGameMessageData
    {
        // ...
    }
}
