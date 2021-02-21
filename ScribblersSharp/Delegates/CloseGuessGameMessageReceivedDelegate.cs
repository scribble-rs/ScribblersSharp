/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "close-guess" game message has been received.
    /// </summary>
    /// <param name="closelyGuessedWord">Closely guessed word</param>
    public delegate void CloseGuessGameMessageReceivedDelegate(string closelyGuessedWord);
}
