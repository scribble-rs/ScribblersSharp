/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents an internal player
    /// </summary>
    internal interface IInternalPlayer : IPlayer
    {
        /// <summary>
        /// Updates player state internally
        /// </summary>
        /// <param name="newName">New player name</param>
        /// <param name="newScore">New score</param>
        /// <param name="newIsConnectedState">New is connected state</param>
        /// <param name="newLastScore">New last score</param>
        /// <param name="newRank">New rank</param>
        /// <param name="newState">New state</param>
        void UpdateInternally(string newName, uint newScore, bool newIsConnectedState, uint newLastScore, uint newRank, EPlayerState newState);

        /// <summary>
        /// Updates player name internally
        /// </summary>
        /// <param name="newName">New player name</param>
        void UpdateNameInternally(string newName);
    }
}
