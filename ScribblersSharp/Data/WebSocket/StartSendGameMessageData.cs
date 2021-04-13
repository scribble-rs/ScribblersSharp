using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "start" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class StartSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Constructs a sendable "start" game message
        /// </summary>
        public StartSendGameMessageData() : base(Naming.GetSendGameMessageDataNameInKebabCase<StartSendGameMessageData>())
        {
            // ...
        }
    }
}
