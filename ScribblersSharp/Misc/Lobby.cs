using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScribblersSharp.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Lobby class
    /// </summary>
    internal class Lobby : ILobby
    {
        /// <summary>
        /// Available game message parsers
        /// </summary>
        private readonly Dictionary<string, List<IBaseGameMessageParser>> gameMessageParsers = new Dictionary<string, List<IBaseGameMessageParser>>();

        /// <summary>
        /// WebSocket receive thread
        /// </summary>
        private Thread webSocketReceiveThread;

        /// <summary>
        /// Received game messages
        /// </summary>
        private readonly ConcurrentQueue<IReceiveGameMessageData> receivedGameMessages = new ConcurrentQueue<IReceiveGameMessageData>();

        /// <summary>
        /// Client web socket
        /// </summary>
        private readonly ClientWebSocket clientWebSocket = new ClientWebSocket();

        /// <summary>
        /// Players
        /// </summary>
        private IPlayer[] players = Array.Empty<IPlayer>();

        /// <summary>
        /// Word hints
        /// </summary>
        private IWordHint[] wordHints = Array.Empty<IWordHint>();

        /// <summary>
        /// Receive buffer
        /// </summary>
        private ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[2048]);

        /// <summary>
        /// Ready game message received event
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Next turn game message received event
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Update players game message received event
        /// </summary>
        public event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Update word hints game message received event
        /// </summary>
        public event UpdateWordHintsGameMessageReceivedDelegate OnUpdateWordHintsGameMessageReceived;

        /// <summary>
        /// Guessing chat message game message received event
        /// </summary>
        public event GuessingChatMessageGameMessageReceivedDelegate OnGuessingChatMessageGameMessageReceived;

        /// <summary>
        /// Non-guessing chat message game message received event
        /// </summary>
        public event NonGuessingChatMessageGameMessageReceivedDelegate OnNonGuessingChatMessageGameMessageReceived;

        /// <summary>
        /// System message game message received event
        /// </summary>
        public event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// Line drawn game message received event
        /// </summary>
        public event LineDrawnGameMessageReceivedDelegate OnLineDrawnGameMessageReceived;

        /// <summary>
        /// Fill drawn game message received event
        /// </summary>
        public event FillDrawnGameMessageReceivedDelegate OnFillDrawnGameMessageReceived;

        /// <summary>
        /// Clear drawing board game message received event
        /// </summary>
        public event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// Your turn game message received event
        /// </summary>
        public event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Correct guess game message received event
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful game message has been received.
        /// </summary>
        public event UnknownGameMessageReceivedDelegate OnUnknownGameMessageReceived;

        /// <summary>
        /// WebSocket state
        /// </summary>
        public WebSocketState WebSocketState => clientWebSocket.State;

        /// <summary>
        /// Lobby ID
        /// </summary>
        public string LobbyID { get; private set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Drawing board base width
        /// </summary>
        public uint DrawingBoardBaseWidth { get; private set; }

        /// <summary>
        /// Drawing board base height
        /// </summary>
        public uint DrawingBoardBaseHeight { get; private set; }

        /// <summary>
        /// Player ID
        /// </summary>
        public string PlayerID { get; private set; } = string.Empty;

        /// <summary>
        /// Is player drawing
        /// </summary>
        public bool IsPlayerDrawing { get; private set; }

        /// <summary>
        /// Round
        /// </summary>
        public uint Round { get; private set; }

        /// <summary>
        /// Maximal rounds
        /// </summary>
        public uint MaximalRounds { get; private set; }

        /// <summary>
        /// Round end time
        /// </summary>
        public long RoundEndTime { get; private set; }

        /// <summary>
        /// Word hints
        /// </summary>
        public IReadOnlyList<IWordHint> WordHints => wordHints;

        /// <summary>
        /// Players
        /// </summary>
        public IReadOnlyList<IPlayer> Players => players;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientWebSocket">Client WebSocket</param>
        /// <param name="username">Username</param>
        /// <param name="lobbyID">LobbyID</param>
        public Lobby(ClientWebSocket clientWebSocket, string username, string lobbyID, uint drawingBoardBaseWidth, uint drawingBoardBaseHeight)
        {
            this.clientWebSocket = clientWebSocket ?? throw new ArgumentNullException(nameof(clientWebSocket));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            LobbyID = lobbyID ?? throw new ArgumentNullException(nameof(lobbyID));
            DrawingBoardBaseWidth = drawingBoardBaseWidth;
            DrawingBoardBaseHeight = drawingBoardBaseHeight;
            AddMessageParser<ReadyReceiveGameMessageData>((gameMessage, json) =>
            {
                ReadyData ready_data = gameMessage.Data;
                PlayerID = ready_data.PlayerID;
                IsPlayerDrawing = ready_data.IsDrawing;
                Round = ready_data.Round;
                MaximalRounds = ready_data.MaximalRounds;
                RoundEndTime = ready_data.RoundEndTime;
                if (ready_data.WordHints == null)
                {
                    wordHints = Array.Empty<IWordHint>();
                }
                else
                {
                    if (wordHints.Length != ready_data.WordHints.Count)
                    {
                        wordHints = new IWordHint[ready_data.WordHints.Count];
                    }
                    Parallel.For(0, wordHints.Length, (index) =>
                    {
                        WordHintData word_hint_data = ready_data.WordHints[index];
                        wordHints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                    });
                }
                if (players.Length != ready_data.Players.Count)
                {
                    players = new IPlayer[ready_data.Players.Count];
                }
                Parallel.For(0, players.Length, (index) =>
                {
                    PlayerData player_data = ready_data.Players[index];
                    players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                });
                List<DrawCommand> draw_commands = new List<DrawCommand>();
                JObject json_object = JObject.Parse(json);
                if (json_object.ContainsKey("data"))
                {
                    if (json_object["data"] is JObject json_data_object)
                    {
                        if (json_data_object.ContainsKey("currentDrawing"))
                        {
                            if (json_data_object["currentDrawing"] is JArray json_draw_commands)
                            {
                                foreach (JToken json_token in json_draw_commands)
                                {
                                    if (json_token is JObject json_draw_command)
                                    {
                                        if (json_draw_command.ContainsKey("lineWidth"))
                                        {
                                            LineData line_data = json_draw_command.ToObject<LineData>();
                                            if (line_data != null)
                                            {
                                                draw_commands.Add(new DrawCommand(EDrawCommandType.Line, line_data.FromX, line_data.FromY, line_data.ToX, line_data.ToY, line_data.Color, line_data.LineWidth));
                                            }
                                        }
                                        else
                                        {
                                            FillData fill_data = json_draw_command.ToObject<FillData>();
                                            if (fill_data != null)
                                            {
                                                draw_commands.Add(new DrawCommand(EDrawCommandType.Fill, fill_data.X, fill_data.Y, default, default, fill_data.Color, 0.0f));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ready_data.CurrentDrawing = draw_commands;
                receivedGameMessages.Enqueue(gameMessage);
            }, MessageParseFailedEvent);
            AddMessageParser<NextTurnReceiveGameMessageData>((gameMessage, json) =>
            {
                NextTurnData next_turn_data = gameMessage.Data;
                IsPlayerDrawing = false;
                if (players.Length != next_turn_data.Players.Length)
                {
                    players = new IPlayer[next_turn_data.Players.Length];
                }
                Parallel.For(0, players.Length, (index) =>
                {
                    PlayerData player_data = next_turn_data.Players[index];
                    players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                });
                receivedGameMessages.Enqueue(gameMessage);
            }, MessageParseFailedEvent);
            AddMessageParser<UpdatePlayersReceiveGameMessageData>((gameMessage, json) =>
            {
                PlayerData[] player_array_data = gameMessage.Data;
                if (players.Length != player_array_data.Length)
                {
                    players = new IPlayer[player_array_data.Length];
                }
                Parallel.For(0, players.Length, (index) =>
                {
                    PlayerData player_data = player_array_data[index];
                    players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                });
                receivedGameMessages.Enqueue(gameMessage);
            }, MessageParseFailedEvent);
            AddMessageParser<UpdateWordhintReceiveGameMessageData>((gameMessage, json) =>
            {
                WordHintData[] word_hint_array_data = gameMessage.Data;
                if (wordHints.Length != word_hint_array_data.Length)
                {
                    wordHints = new IWordHint[word_hint_array_data.Length];
                }
                Parallel.For(0, players.Length, (index) =>
                {
                    WordHintData word_hint_data = word_hint_array_data[index];
                    wordHints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                });
                receivedGameMessages.Enqueue(gameMessage);
            }, MessageParseFailedEvent);
            AddMessageParser<MessageReceiveGameMessageData>((gameMessage, json) => receivedGameMessages.Enqueue(gameMessage), MessageParseFailedEvent);
            AddMessageParser<NonGuessingPlayerMessageReceiveGameMessageData>((gameMessage, json) => receivedGameMessages.Enqueue(gameMessage), MessageParseFailedEvent);
            AddMessageParser<SystemMessageReceiveGameMessageData>((gameMessage, json) => receivedGameMessages.Enqueue(gameMessage), MessageParseFailedEvent);
            AddMessageParser<LineReceiveGameMessageData>((gameMessage, json) => receivedGameMessages.Enqueue(gameMessage), MessageParseFailedEvent);
            AddMessageParser<FillReceiveGameMessageData>((gameMessage, json) => receivedGameMessages.Enqueue(gameMessage), MessageParseFailedEvent);
            AddMessageParser<ClearDrawingBoardReceiveGameMessageData>((gameMessage, json) => receivedGameMessages.Enqueue(gameMessage), MessageParseFailedEvent);
            AddMessageParser<YourTurnReceiveGameMessageData>((gameMessage, json) =>
            {
                IsPlayerDrawing = true;
                receivedGameMessages.Enqueue(gameMessage);
            }, MessageParseFailedEvent);
            AddMessageParser<CorrectGuessReceiveGameMessageData>((gameMessage, json) => receivedGameMessages.Enqueue(gameMessage), MessageParseFailedEvent);
            webSocketReceiveThread = new Thread(async () =>
            {
                using (MemoryStream memory_stream = new MemoryStream())
                {
                    using (StreamReader reader = new StreamReader(memory_stream))
                    {
                        while (this.clientWebSocket.State == WebSocketState.Open)
                        {
                            try
                            {
                                WebSocketReceiveResult result = await this.clientWebSocket.ReceiveAsync(receiveBuffer, default);
                                if (result != null)
                                {
                                    memory_stream.Write(receiveBuffer.Array, 0, result.Count);
                                    if (result.EndOfMessage)
                                    {
                                        memory_stream.Seek(0L, SeekOrigin.Begin);
                                        string json = reader.ReadToEnd();
                                        try
                                        {
                                            BaseGameMessageData base_game_message = JsonConvert.DeserializeObject<BaseGameMessageData>(json);
                                            if (base_game_message != null)
                                            {
                                                if (gameMessageParsers.ContainsKey(base_game_message.MessageType))
                                                {
                                                    foreach (IBaseGameMessageParser game_message_parser in gameMessageParsers[base_game_message.MessageType])
                                                    {
                                                        game_message_parser.ParseMessage(json);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.Error.WriteLine(e);
                                        }
                                        memory_stream.Seek(0L, SeekOrigin.Begin);
                                        memory_stream.SetLength(0L);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.Error.WriteLine(e);
                            }
                        }
                    }
                }
            });
            webSocketReceiveThread.Start();
        }

        /// <summary>
        /// Listens to any message parse failed event
        /// </summary>
        /// <param name="expectedMessageType">Expected message type</param>
        /// <param name="message">Message</param>
        /// <param name="json">JSON</param>
        private void MessageParseFailedEvent<T>(string expectedMessageType, T message, string json) where T : IBaseGameMessageData
        {
            if (message == null)
            {
                Console.Error.WriteLine($"Received message is null of expected message type \"{ expectedMessageType }\".{ Environment.NewLine }{ Environment.NewLine }JSON:{ Environment.NewLine }{ json }");
            }
            else
            {
                Console.Error.WriteLine($"\"Message is invalid. Expected message type: \"{ expectedMessageType }\"; Current message type: { message.MessageType }{ Environment.NewLine }{ Environment.NewLine }JSON:{ Environment.NewLine }{ json }");
            }
        }

        /// <summary>
        /// Send WebSocket message (asynchronous)
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        private Task SendWebSocketMessageAsync<T>(T message)
        {
            Task ret = Task.CompletedTask;
            if (clientWebSocket.State == WebSocketState.Open)
            {
                ret = clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))), WebSocketMessageType.Text, true, default);
            }
            return ret;
        }

        /// <summary>
        /// Adds a game message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onGameMessageParsed">On message parsed</param>
        /// <param name="onGameMessageParseFailed">On message parse failed</param>
        /// <returns>Message parser</returns>
        public IGameMessageParser<T> AddMessageParser<T>(GameMessageParsedDelegate<T> onGameMessageParsed, GameMessageParseFailedDelegate<T> onGameMessageParseFailed = null) where T : IReceiveGameMessageData
        {
            IGameMessageParser<T> ret = new GameMessageParser<T>(onGameMessageParsed, onGameMessageParseFailed);
            List<IBaseGameMessageParser> game_message_parsers;
            if (gameMessageParsers.ContainsKey(ret.MessageType))
            {
                game_message_parsers = gameMessageParsers[ret.MessageType];
            }
            else
            {
                game_message_parsers = new List<IBaseGameMessageParser>();
                gameMessageParsers.Add(ret.MessageType, game_message_parsers);
            }
            game_message_parsers.Add(ret);
            return ret;
        }

        /// <summary>
        /// Removes the specified game message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="gameMessageParser">Message parser</param>
        /// <returns>"true" if message parser was successfully removed, otherwise "false"</returns>
        public bool RemoveMessageParser<T>(IGameMessageParser<T> gameMessageParser) where T : IReceiveGameMessageData
        {
            if (gameMessageParser == null)
            {
                throw new ArgumentNullException(nameof(gameMessageParser));
            }
            bool ret = false;
            if (gameMessageParsers.ContainsKey(gameMessageParser.MessageType))
            {
                List<IBaseGameMessageParser> game_message_parsers = gameMessageParsers[gameMessageParser.MessageType];
                ret = game_message_parsers.Remove(gameMessageParser);
                if (ret && (game_message_parsers.Count <= 0))
                {
                    gameMessageParsers.Remove(gameMessageParser.MessageType);
                }
            }
            return ret;
        }

        /// <summary>
        /// Parses incoming message
        /// </summary>
        /// <param name="json">JSON</param>
        public void ParseMessage(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }
            BaseGameMessageData base_game_message_data = JsonConvert.DeserializeObject<BaseGameMessageData>(json);
            if (base_game_message_data != null)
            {
                if (gameMessageParsers.ContainsKey(base_game_message_data.MessageType))
                {
                    foreach (IBaseGameMessageParser game_message_parser in gameMessageParsers[base_game_message_data.MessageType])
                    {
                        game_message_parser.ParseMessage(json);
                    }
                }
                else
                {
                    OnUnknownGameMessageReceived?.Invoke(base_game_message_data, json);
                }
            }
        }

        /// <summary>
        /// Send start game (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        public Task SendStartGameAsync() => SendWebSocketMessageAsync(new StartSendGameMessageData());

        /// <summary>
        /// Clear drawing board (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        public Task SendClearDrawingBoardAsync() => SendWebSocketMessageAsync(new ClearDrawingBoardSendGameMessageData());

        /// <summary>
        /// Send draw command (asynchronous)
        /// </summary>
        /// <param name="type">Draw command type</param>
        /// <param name="fromX">Draw from X</param>
        /// <param name="fromY">Draw from Y</param>
        /// <param name="toX">Draw to X</param>
        /// <param name="toY">Draw to Y</param>
        /// <param name="color">Draw color</param>
        /// <param name="lineWidth">Line width</param>
        /// <returns>Task</returns>
        public Task SendDrawCommandAsync(EDrawCommandType type, float fromX, float fromY, float toX, float toY, Color color, float lineWidth)
        {
            Task ret = Task.CompletedTask;
            switch (type)
            {
                case EDrawCommandType.Fill:
                    ret = SendWebSocketMessageAsync(new FillSendGameMessageData(fromX, fromY, color));
                    break;
                case EDrawCommandType.Line:
                    ret = SendWebSocketMessageAsync(new LineSendGameMessageData(fromX, fromY, toX, toY, color, lineWidth));
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Send choose word (asynchronous)
        /// </summary>
        /// <param name="index">Choose word index</param>
        /// <returns>Task</returns>
        public Task SendChooseWordAsync(uint index) => SendWebSocketMessageAsync(new ChooseWordSendGameMessageData(index));

        /// <summary>
        /// Send chat message (asynchronous)
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>Task</returns>
        public Task SendChatMessageAsync(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            return SendWebSocketMessageAsync(new MessageSendGameMessageData(content));
        }

        /// <summary>
        /// Process events synchronously
        /// </summary>
        public void ProcessEvents()
        {
            while (receivedGameMessages.TryDequeue(out IReceiveGameMessageData receive_game_message))
            {
                IWordHint[] word_hints;
                IPlayer[] players;
                ChatMessageData chat_message_data;
                switch (receive_game_message)
                {
                    case ReadyReceiveGameMessageData ready_receive_game_message:
                        if (OnReadyGameMessageReceived != null)
                        {
                            ReadyData ready_data = ready_receive_game_message.Data;
                            word_hints = new IWordHint[ready_data.WordHints.Count];
                            players = new IPlayer[ready_data.Players.Count];
                            IDrawCommand[] draw_commands = new IDrawCommand[ready_data.CurrentDrawing.Count];
                            Parallel.For(0, word_hints.Length, (index) =>
                            {
                                WordHintData word_hint_data = ready_data.WordHints[index];
                                word_hints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                            });
                            Parallel.For(0, players.Length, (index) =>
                            {
                                PlayerData player_data = ready_data.Players[index];
                                players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                            });
                            Parallel.For(0, draw_commands.Length, (index) => draw_commands[index] = ready_data.CurrentDrawing[index]);
                            OnReadyGameMessageReceived(ready_data.PlayerID, ready_data.IsDrawing, ready_data.OwnerID, ready_data.Round, ready_data.MaximalRounds, ready_data.RoundEndTime, word_hints, players, draw_commands, ready_data.GameState);
                        }
                        break;
                    case NextTurnReceiveGameMessageData next_turn_receive_game_message:
                        if (OnNextTurnGameMessageReceived != null)
                        {
                            NextTurnData next_turn_data = next_turn_receive_game_message.Data;
                            players = new IPlayer[next_turn_data.Players.Length];
                            Parallel.For(0, players.Length, (index) =>
                            {
                                PlayerData player_data = next_turn_data.Players[index];
                                players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                            });
                            OnNextTurnGameMessageReceived(players, next_turn_data.Round, next_turn_data.RoundEndTime);
                        }
                        break;
                    case UpdatePlayersReceiveGameMessageData update_players_receive_game_message:
                        if (OnUpdatePlayersGameMessageReceived != null)
                        {
                            players = new IPlayer[update_players_receive_game_message.Data.Length];
                            Parallel.For(0, players.Length, (index) =>
                            {
                                PlayerData player_data = update_players_receive_game_message.Data[index];
                                players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                            });
                            OnUpdatePlayersGameMessageReceived(players);
                        }
                        break;
                    case UpdateWordhintReceiveGameMessageData update_word_hints_receive_game_message:
                        if (OnUpdateWordHintsGameMessageReceived != null)
                        {
                            word_hints = new IWordHint[update_word_hints_receive_game_message.Data.Length];
                            Parallel.For(0, word_hints.Length, (index) =>
                            {
                                WordHintData word_hint_data = update_word_hints_receive_game_message.Data[index];
                                word_hints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                            });
                            OnUpdateWordHintsGameMessageReceived(word_hints);
                        }
                        break;
                    case MessageReceiveGameMessageData message_receive_game_message:
                        if (OnGuessingChatMessageGameMessageReceived != null)
                        {
                            chat_message_data = message_receive_game_message.Data;
                            OnGuessingChatMessageGameMessageReceived(chat_message_data.Author, chat_message_data.Content);
                        }
                        break;
                    case NonGuessingPlayerMessageReceiveGameMessageData non_guessing_player_message_receive_game_message:
                        if (OnNonGuessingChatMessageGameMessageReceived != null)
                        {
                            chat_message_data = non_guessing_player_message_receive_game_message.Data;
                            OnNonGuessingChatMessageGameMessageReceived(chat_message_data.Author, chat_message_data.Content);
                        }
                        break;
                    case SystemMessageReceiveGameMessageData system_message_receive_game_message:
                        OnSystemMessageGameMessageReceived?.Invoke(system_message_receive_game_message.Data);
                        break;
                    case LineReceiveGameMessageData line_receive_game_message:
                        if (OnLineDrawnGameMessageReceived != null)
                        {
                            LineData line_data = line_receive_game_message.Data;
                            OnLineDrawnGameMessageReceived(line_data.FromX, line_data.FromY, line_data.ToX, line_data.ToY, line_data.Color, line_data.LineWidth);
                        }
                        break;
                    case FillReceiveGameMessageData fill_receive_game_message:
                        if (OnFillDrawnGameMessageReceived != null)
                        {
                            FillData fill_data = fill_receive_game_message.Data;
                            OnFillDrawnGameMessageReceived(fill_data.X, fill_data.Y, fill_data.Color);
                        }
                        break;
                    case ClearDrawingBoardReceiveGameMessageData _:
                        OnClearDrawingBoardGameMessageReceived?.Invoke();
                        break;
                    case YourTurnReceiveGameMessageData your_turn_receive_game_message:
                        OnYourTurnGameMessageReceived?.Invoke((string[])your_turn_receive_game_message.Data.Clone());
                        break;
                    case CorrectGuessReceiveGameMessageData correct_guess_receive_game_message:
                        OnCorrectGuessGameMessageReceived?.Invoke(correct_guess_receive_game_message.Data);
                        break;
                }
            }
        }

        /// <summary>
        /// Close (asynchronous)
        /// </summary>
        public async void CloseAsync()
        {
            try
            {
                if (clientWebSocket.State == WebSocketState.Open)
                {
                    await clientWebSocket.CloseAsync(WebSocketCloseStatus.Empty, null, default);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            webSocketReceiveThread?.Join();
            webSocketReceiveThread = null;
            clientWebSocket.Dispose();
        }

        /// <summary>
        /// Dispose lobby
        /// </summary>
        public void Dispose() => CloseAsync();
    }
}
