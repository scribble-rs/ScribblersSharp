using System;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Player structure
    /// </summary>
    public struct Player
    {
        /// <summary>
        /// Player ID
        /// </summary>
        public uint ID { get; }

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
        /// Constructor
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <param name="name">Player name</param>
        /// <param name="score">Player score</param>
        /// <param name="isConnected">Is player connected</param>
        /// <param name="lastScore">Player last score</param>
        /// <param name="rank">Player rank</param>
        /// <param name="state">Player state</param>
        internal Player(uint id, string name, uint score, bool isConnected, uint lastScore, uint rank, EPlayerState state)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            ID = id;
            Name = name;
            Score = score;
            IsConnected = isConnected;
            LastScore = lastScore;
            Rank = rank;
            State = state;
        }
    }
}
