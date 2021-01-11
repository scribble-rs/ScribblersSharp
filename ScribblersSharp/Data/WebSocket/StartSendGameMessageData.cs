using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "start" send game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class StartSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public StartSendGameMessageData() : base(Naming.GetSendGameMessageDataNameInKebabCase<StartSendGameMessageData>())
        {
            // ...
        }
    }
}
