/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents a player
    /// </summary>
    public interface IPlayer : IValidable
    {
        /// <summary>
        /// Player ID
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Player name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Player score
        /// </summary>
        uint Score { get; }

        /// <summary>
        /// Is player connected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Player last score
        /// </summary>
        uint LastScore { get; }

        /// <summary>
        /// Player rank
        /// </summary>
        uint Rank { get; }

        /// <summary>
        /// Player state
        /// </summary>
        EPlayerState State { get; }
    }
}
