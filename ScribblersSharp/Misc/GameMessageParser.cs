using Newtonsoft.Json;
using System;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A class that describes a game message parser
    /// </summary>
    /// <typeparam name="T">Game message data type</typeparam>
    internal class GameMessageParser<T> : IGameMessageParser<T> where T : IReceiveGameMessageData
    {
        /// <summary>
        /// Game message type
        /// </summary>
        public string MessageType { get; }

        /// <summary>
        /// On game message parsed
        /// </summary>
        public event GameMessageParsedDelegate<T> OnGameMessageParsed;

        /// <summary>
        /// On game message parse failed
        /// </summary>
        public event GameMessageParseFailedDelegate<T> OnGameMessageParseFailed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onGameMessageParsed">On message parsed</param>
        /// <param name="onGameMessageParseFailed">On message parse failed</param>
        public GameMessageParser(GameMessageParsedDelegate<T> onGameMessageParsed, GameMessageParseFailedDelegate<T> onGameMessageParseFailed)
        {
            MessageType = Naming.GetReceiveGameMessageDataNameInKebabCase<T>();
            OnGameMessageParsed += onGameMessageParsed ?? throw new ArgumentNullException(nameof(onGameMessageParsed));
            if (onGameMessageParseFailed != null)
            {
                OnGameMessageParseFailed += onGameMessageParseFailed;
            }
        }

        /// <summary>
        /// Parses incoming game message
        /// </summary>
        /// <param name="json">JSON</param>
        public void ParseMessage(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }
            T message = JsonConvert.DeserializeObject<T>(json);
            if ((message == null) || !message.IsValid || (message.MessageType != MessageType))
            {
                OnGameMessageParseFailed?.Invoke(MessageType, message, json);
            }
            else
            {
                OnGameMessageParsed?.Invoke(message, json);
            }
        }
    }
}
