using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "request-drawing" game message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class RequestDrawingSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Constructs a sendable "request-drawing" game message
        /// </summary>
        public RequestDrawingSendGameMessageData() : base(Naming.GetSendGameMessageDataNameInKebabCase<RequestDrawingSendGameMessageData>())
        {
            // ...
        }
    }
}
