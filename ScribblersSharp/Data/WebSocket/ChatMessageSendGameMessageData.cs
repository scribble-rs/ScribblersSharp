using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Chat message send game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChatMessageSendGameMessageData : GameMessageData<string>, ISendGameMessageData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Content</param>
        public ChatMessageSendGameMessageData(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            Type = "message";
            Data = content;
        }
    }
}
