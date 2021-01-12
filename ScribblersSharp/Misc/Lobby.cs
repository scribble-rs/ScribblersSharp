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
        /// Received game messages
        /// </summary>
        private readonly ConcurrentQueue<string> receivedGameMessages = new ConcurrentQueue<string>();

        /// <summary>
        /// Client web socket
        /// </summary>
        private readonly ClientWebSocket clientWebSocket = new ClientWebSocket();

        /// <summary>
        /// Players
        /// </summary>
        private readonly Dictionary<string, IPlayer> players = new Dictionary<string, IPlayer>();

        /// <summary>
        /// Current drawing
        /// </summary>
        private readonly List<IDrawCommand> currentDrawing = new List<IDrawCommand>();

        /// <summary>
        /// WebSocket receive thread
        /// </summary>
        private Thread webSocketReceiveThread;

        /// <summary>
        /// Word hints
        /// </summary>
        private IWordHint[] wordHints = Array.Empty<IWordHint>();

        /// <summary>
        /// Receive buffer
        /// </summary>
        private ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[2048]);

        /// <summary>
        /// "ready" game message received event
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// "next-turn" game message received event
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// "update-players" game message received event
        /// </summary>
        public event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// "update-wordhint" game message received event
        /// </summary>
        public event UpdateWordhintGameMessageReceivedDelegate OnUpdateWordhintGameMessageReceived;

        /// <summary>
        /// "message" game message received event
        /// </summary>
        public event MessageGameMessageReceivedDelegate OnMessageGameMessageReceived;

        /// <summary>
        /// "non-guessing-player-message" game message received event
        /// </summary>
        public event NonGuessingPlayerMessageGameMessageReceivedDelegate OnNonGuessingPlayerMessageGameMessageReceived;

        /// <summary>
        /// "system-message" game message received event
        /// </summary>
        public event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// "line" game message received event
        /// </summary>
        public event LineGameMessageReceivedDelegate OnLineGameMessageReceived;

        /// <summary>
        /// "fill" game message received event
        /// </summary>
        public event FillGameMessageReceivedDelegate OnFillGameMessageReceived;

        /// <summary>
        /// "clear-drawing-board" game message received event
        /// </summary>
        public event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// "your-turn" game message received event
        /// </summary>
        public event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// "correct-guess" game message received event
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// "drawing" game message received event
        /// </summary>
        public event DrawingGameMessageReceivedDelegate OnDrawingGameMessageReceived;

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
        /// Drawing board base width
        /// </summary>
        public uint DrawingBoardBaseWidth { get; private set; }

        /// <summary>
        /// Drawing board base height
        /// </summary>
        public uint DrawingBoardBaseHeight { get; private set; }

        /// <summary>
        /// My player
        /// </summary>
        public IPlayer MyPlayer { get; private set; }

        /// <summary>
        /// Is player allowed to draw
        /// </summary>
        public bool IsPlayerAllowedToDraw { get; private set; }

        /// <summary>
        /// Lobby owner
        /// </summary>
        public IPlayer Owner { get; private set; }

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
        public IReadOnlyDictionary<string, IPlayer> Players => players;

        /// <summary>
        /// Current drawing
        /// </summary>
        public IReadOnlyList<IDrawCommand> CurrentDrawing => currentDrawing;

        /// <summary>
        /// Game state
        /// </summary>
        public EGameState GameState { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientWebSocket">Client WebSocket</param>
        /// <param name="lobbyID">LobbyID</param>
        public Lobby(ClientWebSocket clientWebSocket, string lobbyID, uint drawingBoardBaseWidth, uint drawingBoardBaseHeight)
        {
            this.clientWebSocket = clientWebSocket ?? throw new ArgumentNullException(nameof(clientWebSocket));
            LobbyID = lobbyID ?? throw new ArgumentNullException(nameof(lobbyID));
            DrawingBoardBaseWidth = drawingBoardBaseWidth;
            DrawingBoardBaseHeight = drawingBoardBaseHeight;
            AddMessageParser<ReadyReceiveGameMessageData>((gameMessage, json) =>
            {
                ReadyData ready = gameMessage.Data;
                IsPlayerAllowedToDraw = ready.IsPlayerAllowedToDraw;
                Round = ready.Round;
                MaximalRounds = ready.MaximalRounds;
                RoundEndTime = ready.RoundEndTime;
                if (ready.WordHints == null)
                {
                    wordHints = Array.Empty<IWordHint>();
                }
                else
                {
                    if (wordHints.Length != ready.WordHints.Count)
                    {
                        wordHints = new IWordHint[ready.WordHints.Count];
                    }
                    Parallel.For(0, wordHints.Length, (index) =>
                    {
                        WordHintData word_hint_data = ready.WordHints[index];
                        wordHints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                    });
                }
                players.Clear();
                foreach (PlayerData player in ready.Players)
                {
                    players.Add(player.ID, new Player(player.ID, player.Name, player.Score, player.IsConnected, player.LastScore, player.Rank, player.State));
                }
                MyPlayer = players[ready.PlayerID];
                Owner = players[ready.OwnerID];
                currentDrawing.Clear();
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
                                                currentDrawing.Add(new DrawCommand(EDrawCommandType.Line, line_data.FromX, line_data.FromY, line_data.ToX, line_data.ToY, line_data.Color, line_data.LineWidth));
                                            }
                                        }
                                        else
                                        {
                                            FillData fill_data = json_draw_command.ToObject<FillData>();
                                            if (fill_data != null)
                                            {
                                                currentDrawing.Add(new DrawCommand(EDrawCommandType.Fill, fill_data.X, fill_data.Y, default, default, fill_data.Color, 0.0f));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                GameState = ready.GameState;
                OnReadyGameMessageReceived?.Invoke(this);
            }, MessageParseFailedEvent);
            AddMessageParser<NextTurnReceiveGameMessageData>((gameMessage, json) =>
            {
                NextTurnData next_turn = gameMessage.Data;
                IsPlayerAllowedToDraw = false;
                GameState = EGameState.Ongoing;
                Round = next_turn.Round;
                RoundEndTime = next_turn.RoundEndTime;
                players.Clear();
                foreach (PlayerData player_data in next_turn.Players)
                {
                    players.Add(player_data.ID, new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State));
                }
                if (Owner != null)
                {
                    Owner = players.ContainsKey(Owner.ID) ? players[Owner.ID] : null;
                }
                currentDrawing.Clear();
                OnNextTurnGameMessageReceived?.Invoke(this);
            }, MessageParseFailedEvent);
            AddMessageParser<UpdatePlayersReceiveGameMessageData>((gameMessage, json) =>
            {
                players.Clear();
                foreach (PlayerData player in gameMessage.Data)
                {
                    players.Add(player.ID, new Player(player.ID, player.Name, player.Score, player.IsConnected, player.LastScore, player.Rank, player.State));
                }
                if (Owner != null)
                {
                    Owner = players.ContainsKey(Owner.ID) ? players[Owner.ID] : null;
                }
                OnUpdatePlayersGameMessageReceived?.Invoke(players);
            }, MessageParseFailedEvent);
            AddMessageParser<UpdateWordhintReceiveGameMessageData>((gameMessage, json) =>
            {
                GameState = EGameState.Ongoing;
                WordHintData[] word_hints = gameMessage.Data;
                if (wordHints.Length != word_hints.Length)
                {
                    wordHints = new IWordHint[word_hints.Length];
                }
                Parallel.For(0, wordHints.Length, (index) =>
                {
                    WordHintData word_hint_data = word_hints[index];
                    wordHints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                });
                OnUpdateWordhintGameMessageReceived?.Invoke(wordHints);
            }, MessageParseFailedEvent);
            AddMessageParser<MessageReceiveGameMessageData>((gameMessage, json) => OnMessageGameMessageReceived?.Invoke(players.ContainsKey(gameMessage.Data.AuthorID) ? players[gameMessage.Data.AuthorID] : null, gameMessage.Data.Content), MessageParseFailedEvent);
            AddMessageParser<NonGuessingPlayerMessageReceiveGameMessageData>((gameMessage, json) => OnNonGuessingPlayerMessageGameMessageReceived?.Invoke(players.ContainsKey(gameMessage.Data.AuthorID) ? players[gameMessage.Data.AuthorID] : null, gameMessage.Data.Content), MessageParseFailedEvent);
            AddMessageParser<SystemMessageReceiveGameMessageData>((gameMessage, json) => OnSystemMessageGameMessageReceived?.Invoke(gameMessage.Data), MessageParseFailedEvent);
            AddMessageParser<LineReceiveGameMessageData>((gameMessage, json) =>
            {
                LineData line = gameMessage.Data;
                GameState = EGameState.Ongoing;
                currentDrawing.Add(new DrawCommand(EDrawCommandType.Line, line.FromX, line.FromY, line.ToX, line.ToY, line.Color, line.LineWidth));
                OnLineGameMessageReceived?.Invoke(line.FromX, line.FromY, line.ToX, line.ToY, line.Color, line.LineWidth);
            }, MessageParseFailedEvent);
            AddMessageParser<FillReceiveGameMessageData>((gameMessage, json) =>
            {
                FillData fill = gameMessage.Data;
                currentDrawing.Add(new DrawCommand(EDrawCommandType.Fill, fill.X, fill.Y, fill.X, fill.Y, fill.Color, 0.0f));
                OnFillGameMessageReceived(fill.X, fill.Y, fill.Color);
            }, MessageParseFailedEvent);
            AddMessageParser<ClearDrawingBoardReceiveGameMessageData>((gameMessage, json) =>
            {
                currentDrawing.Clear();
                OnClearDrawingBoardGameMessageReceived?.Invoke();
            }, MessageParseFailedEvent);
            AddMessageParser<YourTurnReceiveGameMessageData>((gameMessage, json) =>
            {
                IsPlayerAllowedToDraw = true;
                currentDrawing.Clear();
                OnYourTurnGameMessageReceived?.Invoke((string[])gameMessage.Data.Clone());
            }, MessageParseFailedEvent);
            AddMessageParser<CorrectGuessReceiveGameMessageData>((gameMessage, json) => OnCorrectGuessGameMessageReceived?.Invoke(players.ContainsKey(gameMessage.Data) ? players[gameMessage.Data] : null), MessageParseFailedEvent);
            AddMessageParser<DrawingReceiveGameMessageData>((gameMessage, json) =>
            {
                currentDrawing.Clear();
                JObject json_object = JObject.Parse(json);
                if (json_object.ContainsKey("data"))
                {
                    if (json_object["data"] is JArray json_draw_commands)
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
                                        currentDrawing.Add(new DrawCommand(EDrawCommandType.Line, line_data.FromX, line_data.FromY, line_data.ToX, line_data.ToY, line_data.Color, line_data.LineWidth));
                                    }
                                }
                                else
                                {
                                    FillData fill_data = json_draw_command.ToObject<FillData>();
                                    if (fill_data != null)
                                    {
                                        currentDrawing.Add(new DrawCommand(EDrawCommandType.Fill, fill_data.X, fill_data.Y, default, default, fill_data.Color, 0.0f));
                                    }
                                }
                            }
                        }
                    }
                }
                OnDrawingGameMessageReceived?.Invoke(currentDrawing);
            }, MessageParseFailedEvent);
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
                                        receivedGameMessages.Enqueue(reader.ReadToEnd());
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
                Console.Error.WriteLine($"Message is invalid. Expected message type: \"{ expectedMessageType }\"; Current message type: \"{ message.MessageType }\"{ Environment.NewLine }{ Environment.NewLine }JSON:{ Environment.NewLine }{ json }");
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
        /// Parses incoming message
        /// </summary>
        /// <param name="json">JSON</param>
        private void ParseMessage(string json)
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
        /// Sends a "start" game message (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        public Task SendStartGameMessageAsync() => SendWebSocketMessageAsync(new StartSendGameMessageData());

        /// <summary>
        /// Sends a "name-change" game message (asynchronous)
        /// </summary>
        /// <param name="newUsername">New username</param>
        /// <returns>Task</returns>
        public Task SendNameChangeGameMessageAsync(string newUsername) => SendWebSocketMessageAsync(new NameChangeSendGameMessageData(newUsername));

        /// <summary>
        /// Sends a "request-drawing" game message (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        public Task SendRequestDrawingGameMessageAsync() => SendWebSocketMessageAsync(new RequestDrawingSendGameMessageData());

        /// <summary>
        /// Sends a "clear-drawing-board" game message (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        public Task SendClearDrawingBoardGameMessageAsync() => SendWebSocketMessageAsync(new ClearDrawingBoardSendGameMessageData());

        /// <summary>
        /// Sends a "fill" game message (asynchronous)
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="color"></param>
        /// <returns>Task</returns>
        public Task SendFillGameMessageAsync(float positionX, float positionY, Color color) => SendWebSocketMessageAsync(new FillSendGameMessageData(positionX, positionY, color));

        /// <summary>
        /// Sends a "line" game message (asynchronous)
        /// </summary>
        /// <param name="fromX">Draw from X</param>
        /// <param name="fromY">Draw from Y</param>
        /// <param name="toX">Draw to X</param>
        /// <param name="toY">Draw to Y</param>
        /// <param name="color">Draw color</param>
        /// <param name="lineWidth">Line width</param>
        /// <returns>Task</returns>
        public Task SendLineGameMessageAsync(float fromX, float fromY, float toX, float toY, Color color, float lineWidth) => SendWebSocketMessageAsync(new LineSendGameMessageData(fromX, fromY, toX, toY, color, lineWidth));

        /// <summary>
        /// Sends a "choose-word" game message (asynchronous)
        /// </summary>
        /// <param name="index">Choose word index</param>
        /// <returns>Task</returns>
        public Task SendChooseWordGameMessageAsync(uint index) => SendWebSocketMessageAsync(new ChooseWordSendGameMessageData(index));

        /// <summary>
        /// Sends a "kick-vote" game message (asynchronous)
        /// </summary>
        /// <param name="toKickPlayer">To kick player</param>
        /// <returns></returns>
        public Task SendKickVoteAsync(IPlayer toKickPlayer)
        {
            if (toKickPlayer == null)
            {
                throw new ArgumentNullException(nameof(toKickPlayer));
            }
            return SendWebSocketMessageAsync(new KickVoteSendGameMessageData(toKickPlayer.ID));
        }

        /// <summary>
        /// Sends a "message" game message (asynchronous)
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>Task</returns>
        public Task SendMessageGameMessageAsync(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            return SendWebSocketMessageAsync(new MessageSendGameMessageData(content));
        }

        /// <summary>
        /// Processes events synchronously
        /// </summary>
        public void ProcessEvents()
        {
            while (receivedGameMessages.TryDequeue(out string json))
            {
                try
                {
                    ParseMessage(json);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }

        /// <summary>
        /// Closes lobby
        /// </summary>
        public async void Close()
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
        /// Disposes lobby
        /// </summary>
        public void Dispose() => Close();
    }
}
