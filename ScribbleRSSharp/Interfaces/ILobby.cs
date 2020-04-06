using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.WebSockets;
using System.Numerics;
using System.Threading.Tasks;

/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Lobby interface
    /// </summary>
    public interface ILobby : IDisposable
    {
        /// <summary>
        /// Ready game message received event
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Next turn game message received event
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Update players game message received event
        /// </summary>
        event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Update word hints game message received event
        /// </summary>
        event UpdateWordHintsGameMessageReceivedDelegate OnUpdateWordHintsGameMessageReceived;

        /// <summary>
        /// Guessing chat message game message received event
        /// </summary>
        event GuessingChatMessageGameMessageReceivedDelegate OnGuessingChatMessageGameMessageReceived;

        /// <summary>
        /// Non-guessing chat message game message received event
        /// </summary>
        event NonGuessingChatMessageGameMessageReceivedDelegate OnNonGuessingChatMessageGameMessageReceived;

        /// <summary>
        /// System message game message received event
        /// </summary>
        event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// Line drawn game message received event
        /// </summary>
        event LineDrawnGameMessageReceivedDelegate OnLineDrawnGameMessageReceived;

        /// <summary>
        /// Fill drawn game message received event
        /// </summary>
        event FillDrawnGameMessageReceivedDelegate OnFillDrawnGameMessageReceived;

        /// <summary>
        /// Clear drawing board game message received event
        /// </summary>
        event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// Your turn game message received event
        /// </summary>
        event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// WebSocket state
        /// </summary>
        WebSocketState WebSocketState { get; }

        /// <summary>
        /// Player ID
        /// </summary>
        uint PlayerID { get; }

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
        ulong RoundEndTime { get; }

        /// <summary>
        /// Word hints
        /// </summary>
        IReadOnlyList<WordHint> WordHints { get; }

        /// <summary>
        /// Players
        /// </summary>
        IReadOnlyList<Player> Players { get; }

        /// <summary>
        /// Send start game (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        Task SendStartGameAsync();

        /// <summary>
        /// Clear drawing board (asynchronous)
        /// </summary>
        /// <returns>Task</returns>
        Task SendClearDrawingBoardAsync();

        /// <summary>
        /// Send draw command (asynchronous)
        /// </summary>
        /// <param name="type">Draw command type</param>
        /// <param name="from">Draw from</param>
        /// <param name="to">Draw to</param>
        /// <param name="color">Draw color</param>
        /// <param name="lineWidth">Line width</param>
        /// <returns>Task</returns>
        Task SendDrawCommandAsync(EDrawCommandType type, Vector2 from, Vector2 to, Color color, float lineWidth);

        /// <summary>
        /// Send choose word (asynchronous)
        /// </summary>
        /// <param name="index">Choose word index</param>
        /// <returns>Task</returns>
        Task SendChooseWordAsync(uint index);

        /// <summary>
        /// Send chat message (asynchronous)
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>Task</returns>
        Task SendChatMessageAsync(string content);

        /// <summary>
        /// Process events synchronously
        /// </summary>
        void ProcessEvents();

        /// <summary>
        /// Close (asynchronous)
        /// </summary>
        void CloseAsync();
    }
}
