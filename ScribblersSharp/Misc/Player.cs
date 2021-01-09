using System;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Player structure
    /// </summary>
    internal readonly struct Player : IPlayer
    {
        /// <summary>
        /// Player ID
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// Player name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Player score
        /// </summary>
        public uint Score { get; }

        /// <summary>
        /// Is player connected
        /// </summary>
        public bool IsConnected { get; }

        /// <summary>
        /// Player last score
        /// </summary>
        public uint LastScore { get; }

        /// <summary>
        /// Player rank
        /// </summary>
        public uint Rank { get; }

        /// <summary>
        /// Player state
        /// </summary>
        public EPlayerState State { get; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid => 
            (ID != null) &&
            (Name != null) &&
            (State != EPlayerState.Unknown);

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
            if (state == EPlayerState.Unknown)
            {
                throw new ArgumentException("Player state is unknown.", nameof(state));
            }
            ID = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Score = score;
            IsConnected = isConnected;
            LastScore = lastScore;
            Rank = rank;
            State = state;
        }
    }
}
