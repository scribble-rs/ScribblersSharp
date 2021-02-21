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
        /// Remove player keys
        /// </summary>
        private readonly HashSet<string> removePlayerKeys = new HashSet<string>();

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
        /// WebSocket state
        /// </summary>
        public WebSocketState WebSocketState => clientWebSocket.State;

        /// <summary>
        /// Lobby ID
        /// </summary>
        public string LobbyID { get; private set; }

        /// <summary>
        /// Minimal drawing time in seconds
        /// </summary>
        public uint MinimalDrawingTime { get; }

        /// <summary>
        /// Maximal drawing time in seconds
        /// </summary>
        public uint MaximalDrawingTime { get; }

        /// <summary>
        /// Minimal round count
        /// </summary>
        public uint MinimalRoundCount { get; }

        /// <summary>
        /// Maximal round count
        /// </summary>
        public uint MaximalRoundCount { get; }

        /// <summary>
        /// Minimal of maximal player count
        /// </summary>
        public uint MinimalMaximalPlayerCount { get; }

        /// <summary>
        /// Maximal of maximal player count
        /// </summary>
        public uint MaximalMaximalPlayerCount { get; }

        /// <summary>
        /// Minimal clients per IP count limit
        /// </summary>
        public uint MinimalClientsPerIPLimit { get; }

        /// <summary>
        /// Maximal clients per IP count limit
        /// </summary>
        public uint MaximalClientsPerIPLimit { get; }

        /// <summary>
        /// Maximal player count
        /// </summary>
        public uint MaximalPlayerCount { get; }

        /// <summary>
        /// Is lobby public
        /// </summary>
        public bool IsPublic { get; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        public bool IsVotekickingEnabled { get; private set; }

        /// <summary>
        /// Custom words chance
        /// </summary>
        public uint CustomWordsChance { get; }

        /// <summary>
        /// Clients per IP limit
        /// </summary>
        public uint ClientsPerIPLimit { get; }

        /// <summary>
        /// Drawing board base width
        /// </summary>
        public uint DrawingBoardBaseWidth { get; }

        /// <summary>
        /// Drawing board base height
        /// </summary>
        public uint DrawingBoardBaseHeight { get; }

        /// <summary>
        /// Minimal brush size
        /// </summary>
        public uint MinimalBrushSize { get; }

        /// <summary>
        /// Maximal brush size
        /// </summary>
        public uint MaximalBrushSize { get; }

        /// <summary>
        /// Suggested brush sizes
        /// </summary>
        public IEnumerable<uint> SuggestedBrushSizes { get; }

        /// <summary>
        /// Canvas color
        /// </summary>
        public Color CanvasColor { get; }

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
        /// Previous word
        /// </summary>
        public string PreviousWord { get; private set; } = string.Empty;

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
        /// Gets invoked when a "ready" game message has been received.
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received.
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received.
        /// </summary>
        public event NameChangeMessageReceivedDelegate OnNameChangeMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-players" game message has been received.
        /// </summary>
        public event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-wordhint" game message has been received.
        /// </summary>
        public event UpdateWordhintGameMessageReceivedDelegate OnUpdateWordhintGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "message" game message has been received.
        /// </summary>
        public event MessageGameMessageReceivedDelegate OnMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "non-guessing-player-message" game message has been received.
        /// </summary>
        public event NonGuessingPlayerMessageGameMessageReceivedDelegate OnNonGuessingPlayerMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "system-message" game message has been received.
        /// </summary>
        public event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "line" game message has been received.
        /// </summary>
        public event LineGameMessageReceivedDelegate OnLineGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "fill" game message has been received.
        /// </summary>
        public event FillGameMessageReceivedDelegate OnFillGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "clear-drawing-board" game message has been received.
        /// </summary>
        public event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received.
        /// </summary>
        public event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "close-guess" game message has been received.
        /// </summary>
        public event CloseGuessGameMessageReceivedDelegate OnCloseGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received.
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "kick-vote" game message has been received.
        /// </summary>
        public event KickVoteGameMessageReceivedDelegate OnKickVoteGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "drawer-kicked" game message has been received.
        /// </summary>
        public event DrawerKickedGameMessageReceivedDelegate OnDrawerKickedGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "owner-change" game message has been received.
        /// </summary>
        public event OwnerChangeGameMessageReceivedDelegate OnOwnerChangeGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "drawing" game message has been received.
        /// </summary>
        public event DrawingGameMessageReceivedDelegate OnDrawingGameMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful game message has been received.
        /// </summary>
        public event UnknownGameMessageReceivedDelegate OnUnknownGameMessageReceived;

        /// <summary>
        /// Constructs a lobby
        /// </summary>
        /// <param name="clientWebSocket">Client web socket</param>
        /// <param name="lobbyID">Lobby ID</param>
        /// <param name="minimalDrawingTime">Minimal drawing time in seconds</param>
        /// <param name="maximalDrawingTime">Maximal drawing time in seconds</param>
        /// <param name="minimalRoundCount">Minimal round count</param>
        /// <param name="maximalRoundCount">Maximal round count</param>
        /// <param name="minimalMaximalPlayerCount">Minimal of maximal player count</param>
        /// <param name="maximalMaximalPlayerCount">Maximal of maximal player count</param>
        /// <param name="minimalClientsPerIPLimit">Minimal clients per IP limit</param>
        /// <param name="maximalClientsPerIPLimit">Maximal clients per IP limit</param>
        /// <param name="maximalPlayerCount">Maximal player count</param>
        /// <param name="isPublic">Is lobby public</param>
        /// <param name="isVotekickingEnabled">Is votekicking enabled</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        /// <param name="drawingBoardBaseWidth">Drawing board base width</param>
        /// <param name="drawingBoardBaseHeight">Drawing board base height</param>
        /// <param name="minimalBrushSize">Minimal brush size</param>
        /// <param name="maximalBrushSize">Maximal brush size</param>
        /// <param name="suggestedBrushSizes">Suggested brush sizes</param>
        /// <param name="canvasColor">Canvas color</param>
        public Lobby
        (
            ClientWebSocket clientWebSocket,
            string lobbyID,
            uint minimalDrawingTime,
            uint maximalDrawingTime,
            uint minimalRoundCount,
            uint maximalRoundCount,
            uint minimalMaximalPlayerCount,
            uint maximalMaximalPlayerCount,
            uint minimalClientsPerIPLimit,
            uint maximalClientsPerIPLimit,
            uint maximalPlayerCount,
            bool isPublic,
            bool isVotekickingEnabled,
            uint customWordsChance,
            uint clientsPerIPLimit,
            uint drawingBoardBaseWidth,
            uint drawingBoardBaseHeight,
            uint minimalBrushSize,
            uint maximalBrushSize,
            IEnumerable<uint> suggestedBrushSizes,
            Color canvasColor
        )
        {
            if (minimalDrawingTime < 1U)
            {
                throw new ArgumentException("Minimal drawing time can't be smaller than one.", nameof(minimalDrawingTime));
            }
            if (maximalDrawingTime < minimalDrawingTime)
            {
                throw new ArgumentException("Maximal drawing time can't be smaller than minimal drawing time.", nameof(maximalDrawingTime));
            }
            if (minimalRoundCount < 1U)
            {
                throw new ArgumentException("Minimal round count can't be smaller than one.", nameof(minimalRoundCount));
            }
            if (maximalRoundCount < minimalRoundCount)
            {
                throw new ArgumentException("Maximal round count can't be smaller than minimal round count.", nameof(maximalRoundCount));
            }
            if (minimalMaximalPlayerCount < 1U)
            {
                throw new ArgumentException("Minimal of maximal player count can't be smaller than one.", nameof(minimalMaximalPlayerCount));
            }
            if (maximalMaximalPlayerCount < minimalMaximalPlayerCount)
            {
                throw new ArgumentException("Maximal of maximal player count can't be smaller than minimal of maximal player count.", nameof(maximalMaximalPlayerCount));
            }
            if (minimalClientsPerIPLimit < 1U)
            {
                throw new ArgumentException("Minimal clients per IP limit can't be smaller than one.", nameof(minimalClientsPerIPLimit));
            }
            if (maximalClientsPerIPLimit < minimalClientsPerIPLimit)
            {
                throw new ArgumentException("Maximal clients per IP limit can't be smaller than minimal clients per IP limit.", nameof(maximalClientsPerIPLimit));
            }
            if (maximalPlayerCount < minimalMaximalPlayerCount)
            {
                throw new ArgumentException("Maximal player count can't be smaller than minimal of maximal player count.", nameof(maximalPlayerCount));
            }
            if (maximalPlayerCount > maximalMaximalPlayerCount)
            {
                throw new ArgumentException("Maximal player count can't be bigger than maximal of maximal player count.", nameof(maximalPlayerCount));
            }
            if (clientsPerIPLimit < minimalClientsPerIPLimit)
            {
                throw new ArgumentException("Clients per IP limit can't be smaller than minimal clients per IP limit.", nameof(clientsPerIPLimit));
            }
            if (clientsPerIPLimit > maximalClientsPerIPLimit)
            {
                throw new ArgumentException("Clients per IP limit can't be bigger than maximal clients per IP limit.", nameof(clientsPerIPLimit));
            }
            if (minimalBrushSize < 1U)
            {
                throw new ArgumentException("Minimal brush size can't be smaller than one.", nameof(minimalBrushSize));
            }
            if (maximalBrushSize < minimalBrushSize)
            {
                throw new ArgumentException("Maximal brush size can't be smaller than maximal brush size.", nameof(maximalBrushSize));
            }
            this.clientWebSocket = clientWebSocket ?? throw new ArgumentNullException(nameof(clientWebSocket));
            LobbyID = lobbyID ?? throw new ArgumentNullException(nameof(lobbyID));
            MinimalDrawingTime = minimalDrawingTime;
            MaximalDrawingTime = maximalDrawingTime;
            MinimalRoundCount = minimalRoundCount;
            MaximalRoundCount = maximalRoundCount;
            MinimalMaximalPlayerCount = minimalMaximalPlayerCount;
            MaximalMaximalPlayerCount = maximalMaximalPlayerCount;
            MinimalClientsPerIPLimit = minimalClientsPerIPLimit;
            MaximalClientsPerIPLimit = maximalClientsPerIPLimit;
            MaximalPlayerCount = maximalPlayerCount;
            IsPublic = isPublic;
            IsVotekickingEnabled = isVotekickingEnabled;
            CustomWordsChance = customWordsChance;
            ClientsPerIPLimit = clientsPerIPLimit;
            DrawingBoardBaseWidth = drawingBoardBaseWidth;
            DrawingBoardBaseHeight = drawingBoardBaseHeight;
            MinimalBrushSize = minimalBrushSize;
            MaximalBrushSize = maximalBrushSize;
            SuggestedBrushSizes = suggestedBrushSizes ?? throw new ArgumentNullException(nameof(suggestedBrushSizes));
            CanvasColor = canvasColor;
            AddMessageParser<ReadyReceiveGameMessageData>((gameMessage, json) =>
            {
                ReadyData ready = gameMessage.Data;
                IsPlayerAllowedToDraw = ready.IsPlayerAllowedToDraw;
                IsVotekickingEnabled = ready.IsVotekickingEnabled;
                GameState = ready.GameState;
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
#if SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
                    for (int index = 0; index < wordHints.Length; index++)
#else
                    Parallel.For(0, wordHints.Length, (index) =>
#endif
                    {
                        WordHintData word_hint_data = ready.WordHints[index];
                        wordHints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                    }
#if !SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
                    );
#endif
                }
                UpdateAllPlayers(ready.Players);
                MyPlayer = players.ContainsKey(ready.PlayerID) ? players[ready.PlayerID] : null;
                Owner = players.ContainsKey(ready.OwnerID) ? players[ready.OwnerID] : null;
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
                                ParseCurrentDrawingFromJSON(json_draw_commands);
                            }
                        }
                    }
                }
                OnReadyGameMessageReceived?.Invoke();
            }, MessageParseFailedEvent);
            AddMessageParser<NextTurnReceiveGameMessageData>((gameMessage, json) =>
            {
                NextTurnData next_turn = gameMessage.Data;
                IsPlayerAllowedToDraw = false;
                GameState = EGameState.Ongoing;
                Round = next_turn.Round;
                RoundEndTime = next_turn.RoundEndTime;
                UpdateAllPlayers(next_turn.Players);
                PreviousWord = next_turn.PreviousWord ?? PreviousWord;
                currentDrawing.Clear();
                OnNextTurnGameMessageReceived?.Invoke();
            }, MessageParseFailedEvent);
            AddMessageParser<NameChangeReceiveMessageData>((gameMessage, json) =>
            {
                NameChangeData name_change = gameMessage.Data;
                if (players.ContainsKey(name_change.PlayerID) && players[name_change.PlayerID] is IInternalPlayer internal_player)
                {
                    internal_player.UpdateNameInternally(name_change.PlayerName);
                    OnNameChangeMessageReceived?.Invoke(internal_player);
                }
            }, MessageParseFailedEvent);
            AddMessageParser<UpdatePlayersReceiveGameMessageData>((gameMessage, json) =>
            {
                UpdateAllPlayers(gameMessage.Data);
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
#if SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
                for (int index = 0; index < wordHints.Length; index++)
#else
                Parallel.For(0, wordHints.Length, (index) =>
#endif
                {
                    WordHintData word_hint_data = word_hints[index];
                    wordHints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                }
#if !SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
                );
#endif
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
            AddMessageParser<CloseGuessReceiveGameMessageData>((gameMessage, json) => OnCloseGuessGameMessageReceived?.Invoke(gameMessage.Data), MessageParseFailedEvent);
            AddMessageParser<CorrectGuessReceiveGameMessageData>((gameMessage, json) => OnCorrectGuessGameMessageReceived?.Invoke(players.ContainsKey(gameMessage.Data) ? players[gameMessage.Data] : null), MessageParseFailedEvent);
            AddMessageParser<KickVoteReceiveGameMessageData>
            (
                (gameMessage, json) =>
                {
                    KickVoteData kick_vote = gameMessage.Data;
                    if (players.ContainsKey(kick_vote.PlayerID) && players[kick_vote.PlayerID] is IInternalPlayer internal_player)
                    {
                        internal_player.UpdateNameInternally(kick_vote.PlayerName);
                        OnKickVoteGameMessageReceived?.Invoke(internal_player, kick_vote.VoteCount, kick_vote.RequiredVoteCount);
                    }
                },
                MessageParseFailedEvent
            );
            AddMessageParser<DrawerKickedReceiveGameMessageData>((gameMessage, json) => OnDrawerKickedGameMessageReceived?.Invoke(), MessageParseFailedEvent);
            AddMessageParser<OwnerChangeReceiveGameMessageData>
            (
                (gameMessage, json) =>
                {
                    OwnerChangeData owner_change = gameMessage.Data;
                    if (players.ContainsKey(owner_change.PlayerID) && players[owner_change.PlayerID] is IInternalPlayer internal_player)
                    {
                        internal_player.UpdateNameInternally(owner_change.PlayerName);
                        OnOwnerChangeGameMessageReceived?.Invoke(internal_player);
                    }
                },
                MessageParseFailedEvent
            );
            AddMessageParser<DrawingReceiveGameMessageData>((gameMessage, json) =>
            {
                currentDrawing.Clear();
                JObject json_object = JObject.Parse(json);
                if (json_object.ContainsKey("data"))
                {
                    if (json_object["data"] is JArray json_draw_commands)
                    {
                        ParseCurrentDrawingFromJSON(json_draw_commands);
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
        /// Updates all players
        /// </summary>
        private void UpdateAllPlayers(IEnumerable<PlayerData> players)
        {
            removePlayerKeys.UnionWith(this.players.Keys);
            foreach (PlayerData player_data in players)
            {
                if (this.players.ContainsKey(player_data.ID))
                {
                    if (this.players[player_data.ID] is IInternalPlayer internal_player)
                    {
                        internal_player.UpdateInternally(player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                    }
                    else
                    {
                        this.players[player_data.ID] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                    }
                }
                else
                {
                    this.players.Add(player_data.ID, new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State));
                }
                removePlayerKeys.Remove(player_data.ID);
            }
            foreach (string remove_player_key in removePlayerKeys)
            {
                this.players.Remove(remove_player_key);
            }
            removePlayerKeys.Clear();
            MyPlayer = ((MyPlayer != null) && this.players.ContainsKey(MyPlayer.ID)) ? this.players[MyPlayer.ID] : null;
            Owner = ((Owner != null) && this.players.ContainsKey(Owner.ID)) ? this.players[Owner.ID] : null;
        }

        /// <summary>
        /// Parses current drawing from JSON
        /// </summary>
        /// <param name="jsonDrawCommands">JSON draw commands</param>
        private void ParseCurrentDrawingFromJSON(JArray jsonDrawCommands)
        {
            foreach (JToken json_token in jsonDrawCommands)
            {
                if (json_token is JObject json_draw_command)
                {
                    if (json_draw_command.ContainsKey("type") && json_draw_command.ContainsKey("data") && json_draw_command["data"] is JObject json_draw_command_data)
                    {
                        switch (json_draw_command["type"].ToObject<string>())
                        {
                            case "line":
                                LineData line_data = json_draw_command_data.ToObject<LineData>();
                                if (line_data != null)
                                {
                                    currentDrawing.Add(new DrawCommand(EDrawCommandType.Line, line_data.FromX, line_data.FromY, line_data.ToX, line_data.ToY, line_data.Color, line_data.LineWidth));
                                }
                                break;
                            case "fill":
                                FillData fill_data = json_draw_command_data.ToObject<FillData>();
                                if (fill_data != null)
                                {
                                    currentDrawing.Add(new DrawCommand(EDrawCommandType.Fill, fill_data.X, fill_data.Y, default, default, fill_data.Color, 0.0f));
                                }
                                break;
                        }
                    }
                }
            }
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
            if ((base_game_message_data != null) && base_game_message_data.IsValid)
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
        /// Sends a "start" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        public Task SendStartGameMessageAsync() => SendWebSocketMessageAsync(new StartSendGameMessageData());

        /// <summary>
        /// Sends a "line" game message asynchronously
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
        /// Sends a "fill" game message asynchronously
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="color"></param>
        /// <returns>Task</returns>
        public Task SendFillGameMessageAsync(float positionX, float positionY, Color color) => SendWebSocketMessageAsync(new FillSendGameMessageData(positionX, positionY, color));

        /// <summary>
        /// Sends a "clear-drawing-board" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        public Task SendClearDrawingBoardGameMessageAsync() => SendWebSocketMessageAsync(new ClearDrawingBoardSendGameMessageData());

        /// <summary>
        /// Sends a "message" game message asynchronously
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
        /// Sends a "choose-word" game message asynchronously
        /// </summary>
        /// <param name="index">Choose word index</param>
        /// <returns>Task</returns>
        public Task SendChooseWordGameMessageAsync(uint index) => SendWebSocketMessageAsync(new ChooseWordSendGameMessageData(index));

        /// <summary>
        /// Sends a "name-change" game message asynchronously
        /// </summary>
        /// <param name="newUsername">New username</param>
        /// <returns>Task</returns>
        public Task SendNameChangeGameMessageAsync(string newUsername) => SendWebSocketMessageAsync(new NameChangeSendGameMessageData(newUsername));

        /// <summary>
        /// Sends a "request-drawing" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        public Task SendRequestDrawingGameMessageAsync() => SendWebSocketMessageAsync(new RequestDrawingSendGameMessageData());

        /// <summary>
        /// Sends a "kick-vote" game message asynchronously
        /// </summary>
        /// <param name="toKickPlayer">To kick player</param>
        /// <returns>Task</returns>
        public Task SendKickVoteGameMessageAsync(IPlayer toKickPlayer)
        {
            if (toKickPlayer == null)
            {
                throw new ArgumentNullException(nameof(toKickPlayer));
            }
            return SendWebSocketMessageAsync(new KickVoteSendGameMessageData(toKickPlayer.ID));
        }

        /// <summary>
        /// Sends a "keep-alive" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        public Task SendKeepAliveGameMessageAsync() => SendWebSocketMessageAsync(new KeepAliveSendGameMessageData());

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
