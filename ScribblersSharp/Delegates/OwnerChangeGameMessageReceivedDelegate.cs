/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "owner-change" game message has been received
    /// </summary>
    /// <param name="player">Player</param>
    public delegate void OwnerChangeGameMessageReceivedDelegate(IPlayer player);
}
