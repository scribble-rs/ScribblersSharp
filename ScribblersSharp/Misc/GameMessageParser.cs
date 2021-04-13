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
        /// Gets invoked when parsing game message was successfully
        /// </summary>
        public event GameMessageParsedDelegate<T> OnGameMessageParsed;

        /// <summary>
        /// Gets invoked when parsing game message has failed
        /// </summary>
        public event GameMessageParseFailedDelegate<T> OnGameMessageParseFailed;

        /// <summary>
        /// Constructs a game message parser
        /// </summary>
        /// <param name="onGameMessageParsed">Gets invoked when parsing game message was successfully</param>
        /// <param name="onGameMessageParseFailed">Gets invoked when parsing game message has failed</param>
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
                try
                {
                    OnGameMessageParsed?.Invoke(message, json);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    OnGameMessageParseFailed?.Invoke(MessageType, message, json);
                }
            }
        }
    }
}
