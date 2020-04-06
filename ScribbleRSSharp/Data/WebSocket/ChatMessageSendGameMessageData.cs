using System;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Chat message send game message data class
    /// </summary>
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
