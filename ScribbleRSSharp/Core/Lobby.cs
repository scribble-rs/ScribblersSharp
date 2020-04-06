using ScribblersSharp.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.WebSockets;
using System.Numerics;
using System.Text.Json;
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
        /// WebSocket receive thread
        /// </summary>
        private Thread webSocketReceiveThread;

        /// <summary>
        /// Received game messages
        /// </summary>
        private ConcurrentQueue<IReceiveGameMessageData> receivedGameMessages = new ConcurrentQueue<IReceiveGameMessageData>();

        /// <summary>
        /// Client web socket
        /// </summary>
        private ClientWebSocket clientWebSocket = new ClientWebSocket();

        /// <summary>
        /// Players
        /// </summary>
        private Player[] players = Array.Empty<Player>();

        /// <summary>
        /// Word hints
        /// </summary>
        private WordHint[] wordHints = Array.Empty<WordHint>();

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
        /// WebSocket state
        /// </summary>
        public WebSocketState WebSocketState => clientWebSocket.State;

        /// <summary>
        /// Player ID
        /// </summary>
        public uint PlayerID { get; private set; }

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
        public ulong RoundEndTime { get; private set; }

        /// <summary>
        /// Word hints
        /// </summary>
        public IReadOnlyList<WordHint> WordHints => wordHints;

        /// <summary>
        /// Players
        /// </summary>
        public IReadOnlyList<Player> Players => players;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientWebSocket">Client WebSocket</param>
        /// <param name="username">Username</param>
        /// <param name="lobbyID">LobbyID</param>
        /// <param name="drawingBoardBaseWidth">Drawing board base width</param>
        /// <param name="drawingBoardBaseHeight">Drawing board base height</param>
        public Lobby(ClientWebSocket clientWebSocket, string username, string lobbyID, uint drawingBoardBaseWidth, uint drawingBoardBaseHeight)
        {
            webSocketReceiveThread = new Thread(async () =>
            {
                using (MemoryStream memory_stream = new MemoryStream())
                {
                    while (clientWebSocket.State == WebSocketState.Open)
                    {
                        try
                        {
                            WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(receiveBuffer, default);
                            if (result != null)
                            {
                                if (result.EndOfMessage)
                                {
                                    BaseGameMessageData base_game_message = JsonSerializer.Deserialize<BaseGameMessageData>(receiveBuffer);
                                    if (base_game_message != null)
                                    {
                                        switch (base_game_message.Type)
                                        {
                                            case "ready":
                                                ReadyReceiveGameMessageData ready_game_message = JsonSerializer.Deserialize<ReadyReceiveGameMessageData>(receiveBuffer);
                                                if (ready_game_message != null)
                                                {
                                                    ReadyData ready_data = ready_game_message.Data;
                                                    PlayerID = ready_data.PlayerID;
                                                    IsPlayerDrawing = ready_data.IsDrawing;
                                                    Round = ready_data.Round;
                                                    MaximalRounds = ready_data.MaximalRounds;
                                                    RoundEndTime = ready_data.RoundEndTime;
                                                    if (wordHints.Length != ready_data.WordHints.Length)
                                                    {
                                                        wordHints = new WordHint[ready_data.WordHints.Length];
                                                    }
                                                    Parallel.For(0, wordHints.Length, (index) =>
                                                    {
                                                        WordHintData word_hint_data = ready_data.WordHints[index];
                                                        wordHints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                                                    });
                                                    if (players.Length != ready_data.Players.Length)
                                                    {
                                                        players = new Player[ready_data.Players.Length];
                                                    }
                                                    Parallel.For(0, players.Length, (index) =>
                                                    {
                                                        PlayerData player_data = ready_data.Players[index];
                                                        players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                                                    });
                                                    List<DrawCommand> draw_commands = new List<DrawCommand>();
                                                    JsonDocument json_document = JsonDocument.Parse(receiveBuffer);
                                                    JsonElement json_element = json_document.RootElement;
                                                    if (json_element.TryGetProperty("data", out json_element))
                                                    {
                                                        if (json_element.TryGetProperty("currentDrawing", out json_element))
                                                        {
                                                            foreach (JsonElement draw_command_json_element in json_element.EnumerateArray())
                                                            {
                                                                if (draw_command_json_element.ValueKind == JsonValueKind.Object)
                                                                {
                                                                    if (draw_command_json_element.TryGetProperty("lineWidth", out _))
                                                                    {
                                                                        LineData line_data = JsonSerializer.Deserialize<LineData>(draw_command_json_element.GetRawText());
                                                                        if (line_data != null)
                                                                        {
                                                                            draw_commands.Add(new DrawCommand(EDrawCommandType.Line, new Vector2(line_data.FromX, line_data.FromY), new Vector2(line_data.ToX, line_data.ToY), line_data.Color, line_data.LineWidth));
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        FillData fill_data = JsonSerializer.Deserialize<FillData>(draw_command_json_element.GetRawText());
                                                                        if (fill_data != null)
                                                                        {
                                                                            draw_commands.Add(new DrawCommand(EDrawCommandType.Fill, new Vector2(fill_data.X, fill_data.Y), Vector2.Zero, fill_data.Color, 0.0f));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    ready_data.CurrentDrawing = draw_commands.ToArray();
                                                    draw_commands.Clear();
                                                    receivedGameMessages.Enqueue(ready_game_message);
                                                }
                                                break;
                                            case "next-turn":
                                                NextTurnReceiveGameMessageData next_turn_game_message = JsonSerializer.Deserialize<NextTurnReceiveGameMessageData>(receiveBuffer);
                                                if (next_turn_game_message != null)
                                                {
                                                    NextTurnData next_turn_data = next_turn_game_message.Data;
                                                    if (players.Length != next_turn_data.Players.Length)
                                                    {
                                                        players = new Player[next_turn_data.Players.Length];
                                                    }
                                                    Parallel.For(0, players.Length, (index) =>
                                                    {
                                                        PlayerData player_data = next_turn_data.Players[index];
                                                        players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                                                    });
                                                    receivedGameMessages.Enqueue(next_turn_game_message);
                                                }
                                                break;
                                            case "update-players":
                                                UpdatePlayersReceiveGameMessageData update_players_game_message = JsonSerializer.Deserialize<UpdatePlayersReceiveGameMessageData>(receiveBuffer);
                                                if (update_players_game_message != null)
                                                {
                                                    PlayerData[] player_array_data = update_players_game_message.Data;
                                                    if (players.Length != player_array_data.Length)
                                                    {
                                                        players = new Player[player_array_data.Length];
                                                    }
                                                    Parallel.For(0, players.Length, (index) =>
                                                    {
                                                        PlayerData player_data = player_array_data[index];
                                                        players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                                                    });
                                                    receivedGameMessages.Enqueue(update_players_game_message);
                                                }
                                                break;
                                            case "update-wordhint":
                                                UpdateWordHintsReceiveGameMessageData update_word_hints_game_message = JsonSerializer.Deserialize<UpdateWordHintsReceiveGameMessageData>(receiveBuffer);
                                                if (update_word_hints_game_message != null)
                                                {
                                                    WordHintData[] word_hint_array_data = update_word_hints_game_message.Data;
                                                    if (wordHints.Length != word_hint_array_data.Length)
                                                    {
                                                        wordHints = new WordHint[word_hint_array_data.Length];
                                                    }
                                                    Parallel.For(0, players.Length, (index) =>
                                                    {
                                                        WordHintData word_hint_data = word_hint_array_data[index];
                                                        wordHints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                                                    });
                                                    receivedGameMessages.Enqueue(update_word_hints_game_message);
                                                }
                                                break;
                                            case "message":
                                                GuessingChatMessageReceiveGameMessageData guessing_chat_message_game_message = JsonSerializer.Deserialize<GuessingChatMessageReceiveGameMessageData>(receiveBuffer);
                                                if (guessing_chat_message_game_message != null)
                                                {
                                                    receivedGameMessages.Enqueue(guessing_chat_message_game_message);
                                                }
                                                break;
                                            case "non-guessing-player-message":
                                                NonGuessingChatMessageReceiveGameMessageData non_guessing_chat_message_game_message = JsonSerializer.Deserialize<NonGuessingChatMessageReceiveGameMessageData>(receiveBuffer);
                                                if (non_guessing_chat_message_game_message != null)
                                                {
                                                    receivedGameMessages.Enqueue(non_guessing_chat_message_game_message);
                                                }
                                                break;
                                            case "system-message":
                                                SystemMessageReceiveGameMessageData system_message_game_message = JsonSerializer.Deserialize<SystemMessageReceiveGameMessageData>(receiveBuffer);
                                                if (system_message_game_message != null)
                                                {
                                                    receivedGameMessages.Enqueue(system_message_game_message);
                                                }
                                                break;
                                            case "line":
                                                LineDrawReceiveGameMessageData line_game_message = JsonSerializer.Deserialize<LineDrawReceiveGameMessageData>(receiveBuffer);
                                                if (line_game_message != null)
                                                {
                                                    receivedGameMessages.Enqueue(line_game_message);
                                                }
                                                break;
                                            case "fill":
                                                FillDrawReceiveGameMessageData fill_game_message = JsonSerializer.Deserialize<FillDrawReceiveGameMessageData>(receiveBuffer);
                                                if (fill_game_message != null)
                                                {
                                                    receivedGameMessages.Enqueue(fill_game_message);
                                                }
                                                break;
                                            case "clear-drawing-board":
                                                ClearDrawingBoardReceiveGameMessageData clear_drawing_board_game_message = JsonSerializer.Deserialize<ClearDrawingBoardReceiveGameMessageData>(receiveBuffer);
                                                if (clear_drawing_board_game_message != null)
                                                {
                                                    receivedGameMessages.Enqueue(clear_drawing_board_game_message);
                                                }
                                                break;
                                            case "your-turn":
                                                YourTurnReceiveGameMessageData your_turn_game_message = JsonSerializer.Deserialize<YourTurnReceiveGameMessageData>(receiveBuffer);
                                                if (your_turn_game_message != null)
                                                {
                                                    receivedGameMessages.Enqueue(your_turn_game_message);
                                                }
                                                break;
                                        }
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
            });
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
                ret = clientWebSocket.SendAsync(new ArraySegment<byte>(JsonSerializer.SerializeToUtf8Bytes(message)), WebSocketMessageType.Text, true, default);
            }
            return ret;
        }

        /// <summary>
        /// Send start game (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        public Task SendStartGameAsync() => SendWebSocketMessageAsync(new GameStartSendGameMessageData());

        /// <summary>
        /// Clear drawing board (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        public Task SendClearDrawingBoardAsync() => SendWebSocketMessageAsync(new ClearDrawingBoardSendGameMessageData());

        /// <summary>
        /// Send draw command (asynchronous)
        /// </summary>
        /// <param name="type">Draw command type</param>
        /// <param name="from">Draw from</param>
        /// <param name="to">Draw to</param>
        /// <param name="color">Draw color</param>
        /// <param name="lineWidth">Line width</param>
        /// <returns>Task</returns>
        public Task SendDrawCommandAsync(EDrawCommandType type, Vector2 from, Vector2 to, Color color, float lineWidth)
        {
            Task ret = Task.CompletedTask;
            switch (type)
            {
                case EDrawCommandType.Fill:
                    ret = SendWebSocketMessageAsync(new FillDrawSendGameMessageData(from.X, from.Y, color));
                    break;
                case EDrawCommandType.Line:
                    ret = SendWebSocketMessageAsync(new LineDrawSendGameMessageData(from.X, from.Y, to.X, to.Y, color, lineWidth));
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
            return SendWebSocketMessageAsync(new ChatMessageSendGameMessageData(content));
        }

        /// <summary>
        /// Process events synchronously
        /// </summary>
        public void ProcessEvents()
        {
            IReceiveGameMessageData receive_game_message;
            while (receivedGameMessages.TryDequeue(out receive_game_message))
            {
                WordHint[] word_hints;
                Player[] players;
                ChatMessageData chat_message_data;
                switch (receive_game_message)
                {
                    case ReadyReceiveGameMessageData ready_receive_game_message:
                        if (OnReadyGameMessageReceived != null)
                        {
                            ReadyData ready_data = ready_receive_game_message.Data;
                            word_hints = new WordHint[ready_data.WordHints.Length];
                            players = new Player[ready_data.Players.Length];
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
                            OnReadyGameMessageReceived(ready_data.PlayerID, ready_data.IsDrawing, ready_data.Round, ready_data.MaximalRounds, ready_data.RoundEndTime, word_hints, players, ready_data.CurrentDrawing);
                        }
                        break;
                    case NextTurnReceiveGameMessageData next_turn_game_message:
                        if (OnNextTurnGameMessageReceived != null)
                        {
                            NextTurnData next_turn_data = next_turn_game_message.Data;
                            players = new Player[next_turn_data.Players.Length];
                            Parallel.For(0, players.Length, (index) =>
                            {
                                PlayerData player_data = next_turn_data.Players[index];
                                players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                            });
                            OnNextTurnGameMessageReceived(players, next_turn_data.Round, next_turn_data.RoundEndTime);
                        }
                        break;
                    case UpdatePlayersReceiveGameMessageData update_players_game_message:
                        if (OnUpdatePlayersGameMessageReceived != null)
                        {
                            players = new Player[update_players_game_message.Data.Length];
                            Parallel.For(0, players.Length, (index) =>
                            {
                                PlayerData player_data = update_players_game_message.Data[index];
                                players[index] = new Player(player_data.ID, player_data.Name, player_data.Score, player_data.IsConnected, player_data.LastScore, player_data.Rank, player_data.State);
                            });
                            OnUpdatePlayersGameMessageReceived(players);
                        }
                        break;
                    case UpdateWordHintsReceiveGameMessageData update_word_hints_game_message:
                        if (OnUpdateWordHintsGameMessageReceived != null)
                        {
                            word_hints = new WordHint[update_word_hints_game_message.Data.Length];
                            Parallel.For(0, word_hints.Length, (index) =>
                            {
                                WordHintData word_hint_data = update_word_hints_game_message.Data[index];
                                word_hints[index] = new WordHint(word_hint_data.Character, word_hint_data.Underline);
                            });
                            OnUpdateWordHintsGameMessageReceived(word_hints);
                        }
                        break;
                    case GuessingChatMessageReceiveGameMessageData guessing_chat_message_game_message:
                        if (OnGuessingChatMessageGameMessageReceived != null)
                        {
                            chat_message_data = guessing_chat_message_game_message.Data;
                            OnGuessingChatMessageGameMessageReceived(chat_message_data.Author, chat_message_data.Content);
                        }
                        break;
                    case NonGuessingChatMessageReceiveGameMessageData non_guessing_chat_message_game_message:
                        if (OnNonGuessingChatMessageGameMessageReceived != null)
                        {
                            chat_message_data = non_guessing_chat_message_game_message.Data;
                            OnNonGuessingChatMessageGameMessageReceived(chat_message_data.Author, chat_message_data.Content);
                        }
                        break;
                    case SystemMessageReceiveGameMessageData system_message_game_message:
                        OnSystemMessageGameMessageReceived?.Invoke(system_message_game_message.Data);
                        break;
                    case LineDrawReceiveGameMessageData line_draw_game_message:
                        if (OnLineDrawnGameMessageReceived != null)
                        {
                            LineData line_data = line_draw_game_message.Data;
                            OnLineDrawnGameMessageReceived(new Vector2(line_data.FromX, line_data.FromY), new Vector2(line_data.ToX, line_data.ToY), line_data.Color, line_data.LineWidth);
                        }
                        break;
                    case FillDrawReceiveGameMessageData fill_draw_game_message:
                        if (OnFillDrawnGameMessageReceived != null)
                        {
                            FillData fill_data = fill_draw_game_message.Data;
                            OnFillDrawnGameMessageReceived(new Vector2(fill_data.X, fill_data.Y), fill_data.Color);
                        }
                        break;
                    case ClearDrawingBoardReceiveGameMessageData _:
                        OnClearDrawingBoardGameMessageReceived?.Invoke();
                        break;
                    case YourTurnReceiveGameMessageData your_turn_game_message:
                        OnYourTurnGameMessageReceived((string[])(your_turn_game_message.Data.Clone()));
                        break;
                }
            }
        }

        /// <summary>
        /// Close (asynchronous)
        /// </summary>
        public async void CloseAsync()
        {
            await clientWebSocket.CloseAsync(WebSocketCloseStatus.Empty, "disconnect", default);
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
