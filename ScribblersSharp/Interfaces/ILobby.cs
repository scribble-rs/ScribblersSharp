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
        /// "ready" game message received event
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// "next-turn" game message received event
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// "update-players" game message received event
        /// </summary>
        event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// "update-wordhint" game message received event
        /// </summary>
        event UpdateWordhintGameMessageReceivedDelegate OnUpdateWordhintGameMessageReceived;

        /// <summary>
        /// "message" game message received event
        /// </summary>
        event MessageGameMessageReceivedDelegate OnMessageGameMessageReceived;

        /// <summary>
        /// "non-guessing-player-message" game message received event
        /// </summary>
        event NonGuessingPlayerMessageGameMessageReceivedDelegate OnNonGuessingPlayerMessageGameMessageReceived;

        /// <summary>
        /// "system-message" game message received event
        /// </summary>
        event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// "line" game message received event
        /// </summary>
        event LineGameMessageReceivedDelegate OnLineGameMessageReceived;

        /// <summary>
        /// "fill" game message received event
        /// </summary>
        event FillGameMessageReceivedDelegate OnFillGameMessageReceived;

        /// <summary>
        /// "clear-drawing-board" game message received event
        /// </summary>
        event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// "your-turn" game message received event
        /// </summary>
        event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// "correct-guess" game message received event
        /// </summary>
        event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// "drawing" game message received event
        /// </summary>
        event DrawingGameMessageReceivedDelegate OnDrawingGameMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful game message has been received.
        /// </summary>
        event UnknownGameMessageReceivedDelegate OnUnknownGameMessageReceived;

        /// <summary>
        /// WebSocket state
        /// </summary>
        WebSocketState WebSocketState { get; }

        /// <summary>
        /// Lobby ID
        /// </summary>
        string LobbyID { get; }

        /// <summary>
        /// Username
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Drawing board base width
        /// </summary>
        uint DrawingBoardBaseWidth { get; }

        /// <summary>
        /// Drawing board base height
        /// </summary>
        uint DrawingBoardBaseHeight { get; }

        /// <summary>
        /// Player ID
        /// </summary>
        string PlayerID { get; }

        /// <summary>
        /// Is player drawing
        /// </summary>
        bool IsPlayerDrawing { get; }

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
        IReadOnlyList<IPlayer> Players { get; }

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
        /// Sends a "start" game message (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        Task SendStartGameMessageAsync();

        /// <summary>
        /// Sends a "name-change" game message (asynchronous)
        /// </summary>
        /// <param name="newUsername">New username</param>
        /// <returns>Task</returns>
        Task SendNameChangeGameMessageAsync(string newUsername);

        /// <summary>
        /// Sends a "request-drawing" game message (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        Task SendRequestDrawingGameMessageAsync();

        /// <summary>
        /// Sends a "clear-drawing-board" game message (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        Task SendClearDrawingBoardGameMessageAsync();

        /// <summary>
        /// Sends a "fill" game message (asynchronous)
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="color"></param>
        /// <returns>Task</returns>
        Task SendFillGameMessageAsync(float positionX, float positionY, Color color);

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
        Task SendLineGameMessageAsync(float fromX, float fromY, float toX, float toY, Color color, float lineWidth);

        /// <summary>
        /// Sends a "choose-word" game message (asynchronous)
        /// </summary>
        /// <param name="index">Choose word index</param>
        /// <returns>Task</returns>
        Task SendChooseWordGameMessageAsync(uint index);

        /// <summary>
        /// Sends a "kick-vote" game message (asynchronous)
        /// </summary>
        /// <param name="toKickPlayer">To kick player</param>
        /// <returns></returns>
        Task SendKickVoteAsync(IPlayer toKickPlayer);

        /// <summary>
        /// Sends a "message" game message (asynchronous)
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>Task</returns>
        Task SendMessageGameMessageAsync(string content);

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
