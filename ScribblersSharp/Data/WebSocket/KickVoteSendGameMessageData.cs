using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "kick-vote" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class KickVoteSendGameMessageData : GameMessageData<string>, ISendGameMessageData
    {
        /// <summary>
        /// Constructs a sendable "kick-vote" game message
        /// </summary>
        /// <param name="toKickPlayerID">To kick player ID</param>
        public KickVoteSendGameMessageData(string toKickPlayerID) : base(Naming.GetSendGameMessageDataNameInKebabCase<KickVoteSendGameMessageData>(), toKickPlayerID)
        {
            // ...
        }
    }
}
