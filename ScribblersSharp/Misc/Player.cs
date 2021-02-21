using System;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A class that describes a player
    /// </summary>
    internal class Player : IInternalPlayer
    {
        /// <summary>
        /// Player ID
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Player name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Player score
        /// </summary>
        public uint Score { get; private set; }

        /// <summary>
        /// Is player connected
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Player last score
        /// </summary>
        public uint LastScore { get; private set; }

        /// <summary>
        /// Player rank
        /// </summary>
        public uint Rank { get; private set; }

        /// <summary>
        /// Player state
        /// </summary>
        public EPlayerState State { get; private set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid => 
            (ID != null) &&
            (Name != null) &&
            (State != EPlayerState.Invalid);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="name">Player name</param>
        /// <param name="score">Player score</param>
        /// <param name="isConnected">Is player connected</param>
        /// <param name="lastScore">Player last score</param>
        /// <param name="rank">Player rank</param>
        /// <param name="state">Player state</param>
        public Player(string id, string name, uint score, bool isConnected, uint lastScore, uint rank, EPlayerState state)
        {
            ID = id ?? throw new ArgumentNullException(nameof(id));
            UpdateInternally(name, score, isConnected, lastScore, rank, state);
        }

        /// <summary>
        /// Updates player state internally
        /// </summary>
        /// <param name="newName">New player name</param>
        /// <param name="newScore">New score</param>
        /// <param name="newIsConnectedState">New is connected state</param>
        /// <param name="newLastScore">New last score</param>
        /// <param name="newRank">New rank</param>
        /// <param name="newState">New state</param>
        public void UpdateInternally(string newName, uint newScore, bool newIsConnectedState, uint newLastScore, uint newRank, EPlayerState newState)
        {
            if (newState == EPlayerState.Invalid)
            {
                throw new ArgumentException("Player state is unknown.", nameof(newState));
            }
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
            Score = newScore;
            IsConnected = newIsConnectedState;
            LastScore = newLastScore;
            Rank = newRank;
            State = newState;
        }

        /// <summary>
        /// Updates player name internally
        /// </summary>
        /// <param name="newName">New player name</param>
        public void UpdateNameInternally(string newName) => Name = newName ?? throw new ArgumentNullException(nameof(newName));
    }
}
