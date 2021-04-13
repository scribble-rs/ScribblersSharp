/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Used to signal when a "fill" game message has been received
    /// </summary>
    /// <param name="positionX">Fill position X</param>
    /// <param name="positionY">Fill position Y</param>
    /// <param name="color">Fill color</param>
    public delegate void FillGameMessageReceivedDelegate(float positionX, float positionY, Color color);
}
