using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "kick-vote" send game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class KickVoteSendGameMessageData : GameMessageData<string>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="toKickPlayerID">To kick player ID</param>
        public KickVoteSendGameMessageData(string toKickPlayerID) : base(Naming.GetSendGameMessageDataNameInKebabCase<KickVoteSendGameMessageData>(), toKickPlayerID)
        {
            // ...
        }
    }
}
