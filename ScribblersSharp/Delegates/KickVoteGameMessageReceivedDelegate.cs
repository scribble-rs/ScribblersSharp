/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "kick-vote" game message has been received
    /// </summary>
    /// <param name="player">Player to be kicked</param>
    /// <param name="voteCount">Vote count</param>
    /// <param name="requiredVoteCount">Required vote count</param>
    public delegate void KickVoteGameMessageReceivedDelegate(IPlayer player, uint voteCount, uint requiredVoteCount);
}
