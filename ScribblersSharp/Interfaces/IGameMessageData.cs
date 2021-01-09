/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// AN interface that represents game message data
    /// </summary>
    /// <typeparam name="T">Message data type</typeparam>
    public interface IGameMessageData<T> : IValidable
    {
        /// <summary>
        /// Game message data
        /// </summary>
        T Data { get; }
    }
}
