using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// "request-drawing" receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class RequestDrawingSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RequestDrawingSendGameMessageData() : base(Naming.GetSendGameMessageDataNameInKebabCase<RequestDrawingSendGameMessageData>())
        {
            // ...
        }
    }
}
