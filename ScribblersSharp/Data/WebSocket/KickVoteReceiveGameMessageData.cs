using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a received "kick-vote" game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class KickVoteReceiveGameMessageData : GameMessageData<KickVoteData>, IReceiveGameMessageData
    {
        // ...
    }
}
