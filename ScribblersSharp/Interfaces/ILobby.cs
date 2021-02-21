using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.WebSockets;
using System.Threading.Tasks;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Lobby interface
    /// </summary>
    public interface ILobby : IDisposable
    {
        /// <summary>
        /// WebSocket state
        /// </summary>
        WebSocketState WebSocketState { get; }

        /// <summary>
        /// Lobby ID
        /// </summary>
        string LobbyID { get; }

        /// <summary>
        /// Minimal drawing time in seconds
        /// </summary>
        uint MinimalDrawingTime { get; }

        /// <summary>
        /// Maximal drawing time in seconds
        /// </summary>
        uint MaximalDrawingTime { get; }

        /// <summary>
        /// Minimal round count
        /// </summary>
        uint MinimalRoundCount { get; }

        /// <summary>
        /// Maximal round count
        /// </summary>
        uint MaximalRoundCount { get; }

        /// <summary>
        /// Minimal of maximal player count
        /// </summary>
        uint MinimalMaximalPlayerCount { get; }

        /// <summary>
        /// Maximal of maximal player count
        /// </summary>
        uint MaximalMaximalPlayerCount { get; }

        /// <summary>
        /// Minimal clients per IP count limit
        /// </summary>
        uint MinimalClientsPerIPLimit { get; }

        /// <summary>
        /// Maximal clients per IP count limit
        /// </summary>
        uint MaximalClientsPerIPLimit { get; }

        /// <summary>
        /// Maximal player count
        /// </summary>
        uint MaximalPlayerCount { get; }

        /// <summary>
        /// Is lobby public
        /// </summary>
        bool IsPublic { get; }

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        bool IsVotekickingEnabled { get; }

        /// <summary>
        /// Custom words chance
        /// </summary>
        uint CustomWordsChance { get; }

        /// <summary>
        /// Clients per IP limit
        /// </summary>
        uint ClientsPerIPLimit { get; }

        /// <summary>
        /// Drawing board base width
        /// </summary>
        uint DrawingBoardBaseWidth { get; }

        /// <summary>
        /// Drawing board base height
        /// </summary>
        uint DrawingBoardBaseHeight { get; }

        /// <summary>
        /// Minimal brush size
        /// </summary>
        uint MinimalBrushSize { get; }

        /// <summary>
        /// Maximal brush size
        /// </summary>
        uint MaximalBrushSize { get; }

        /// <summary>
        /// Suggested brush sizes
        /// </summary>
        IEnumerable<uint> SuggestedBrushSizes { get; }

        /// <summary>
        /// Canvas color
        /// </summary>
        Color CanvasColor { get; }

        /// <summary>
        /// My player
        /// </summary>
        IPlayer MyPlayer { get; }

        /// <summary>
        /// Is player allowed to draw
        /// </summary>
        bool IsPlayerAllowedToDraw { get; }

        /// <summary>
        /// Lobby owner
        /// </summary>
        IPlayer Owner { get; }

        /// <summary>
        /// Round
        /// </summary>
        uint Round { get; }

        /// <summary>
        /// Maximal rounds
        /// </summary>
        uint MaximalRounds { get; }

        /// <summary>
        /// Round end time
        /// </summary>
        long RoundEndTime { get; }

        /// <summary>
        /// Word hints
        /// </summary>
        IReadOnlyList<IWordHint> WordHints { get; }

        /// <summary>
        /// Players
        /// </summary>
        IReadOnlyDictionary<string, IPlayer> Players { get; }

        /// <summary>
        /// Current drawing
        /// </summary>
        IReadOnlyList<IDrawCommand> CurrentDrawing { get; }

        /// <summary>
        /// Game state
        /// </summary>
        EGameState GameState { get; }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received.
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received.
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received.
        /// </summary>
        event NameChangeGameMessageReceivedDelegate OnNameChangeGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-players" game message has been received.
        /// </summary>
        event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-wordhint" game message has been received.
        /// </summary>
        event UpdateWordhintGameMessageReceivedDelegate OnUpdateWordhintGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "message" game message has been received.
        /// </summary>
        event MessageGameMessageReceivedDelegate OnMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "non-guessing-player-message" game message has been received.
        /// </summary>
        event NonGuessingPlayerMessageGameMessageReceivedDelegate OnNonGuessingPlayerMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "system-message" game message has been received.
        /// </summary>
        event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "line" game message has been received.
        /// </summary>
        event LineGameMessageReceivedDelegate OnLineGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "fill" game message has been received.
        /// </summary>
        event FillGameMessageReceivedDelegate OnFillGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "clear-drawing-board" game message has been received.
        /// </summary>
        event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received.
        /// </summary>
        event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "close-guess" game message has been received.
        /// </summary>
        event CloseGuessGameMessageReceivedDelegate OnCloseGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received.
        /// </summary>
        event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "kick-vote" game message has been received.
        /// </summary>
        event KickVoteGameMessageReceivedDelegate OnKickVoteGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "drawer-kicked" game message has been received.
        /// </summary>
        event DrawerKickedGameMessageReceivedDelegate OnDrawerKickedGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "owner-change" game message has been received.
        /// </summary>
        event OwnerChangeGameMessageReceivedDelegate OnOwnerChangeGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "drawing" game message has been received.
        /// </summary>
        event DrawingGameMessageReceivedDelegate OnDrawingGameMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful game message has been received.
        /// </summary>
        event UnknownGameMessageReceivedDelegate OnUnknownGameMessageReceived;

        /// <summary>
        /// Adds a game message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onGameMessageParsed">On message parsed</param>
        /// <param name="onGameMessageParseFailed">On message parse failed</param>
        /// <returns>Message parser</returns>
        IGameMessageParser<T> AddMessageParser<T>(GameMessageParsedDelegate<T> onGameMessageParsed, GameMessageParseFailedDelegate<T> onGameMessageParseFailed = null) where T : IReceiveGameMessageData;

        /// <summary>
        /// Removes the specified game message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="gameMessageParser">Message parser</param>
        /// <returns>"true" if message parser was successfully removed, otherwise "false"</returns>
        bool RemoveMessageParser<T>(IGameMessageParser<T> gameMessageParser) where T : IReceiveGameMessageData;

        /// <summary>
        /// Sends a "start" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        Task SendStartGameMessageAsync();

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
        Task SendLineGameMessageAsync(float fromX, float fromY, float toX, float toY, Color color, float lineWidth);

        /// <summary>
        /// Sends a "fill" game message asynchronously
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="color"></param>
        /// <returns>Task</returns>
        Task SendFillGameMessageAsync(float positionX, float positionY, Color color);

        /// <summary>
        /// Sends a "clear-drawing-board" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        Task SendClearDrawingBoardGameMessageAsync();

        /// <summary>
        /// Sends a "message" game message asynchronously
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>Task</returns>
        Task SendMessageGameMessageAsync(string content);

        /// <summary>
        /// Sends a "choose-word" game message asynchronously
        /// </summary>
        /// <param name="index">Choose word index</param>
        /// <returns>Task</returns>
        Task SendChooseWordGameMessageAsync(uint index);

        /// <summary>
        /// Sends a "name-change" game message asynchronously
        /// </summary>
        /// <param name="newUsername">New username</param>
        /// <returns>Task</returns>
        Task SendNameChangeGameMessageAsync(string newUsername);

        /// <summary>
        /// Sends a "request-drawing" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        Task SendRequestDrawingGameMessageAsync();

        /// <summary>
        /// Sends a "kick-vote" game message asynchronously
        /// </summary>
        /// <param name="toKickPlayer">To kick player</param>
        /// <returns>Task</returns>
        Task SendKickVoteGameMessageAsync(IPlayer toKickPlayer);

        /// <summary>
        /// Sends a "keep-alive" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        Task SendKeepAliveGameMessageAsync();

        /// <summary>
        /// Processes events synchronously
        /// </summary>
        void ProcessEvents();

        /// <summary>
        /// Closes lobby
        /// </summary>
        void Close();
    }
}
