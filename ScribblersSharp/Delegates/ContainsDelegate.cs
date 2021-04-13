/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A delegate to check if collection contains an element
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="element">Element</param>
    /// <returns>"true" if element is contained, otherwise "false"</returns>
    internal delegate bool ContainsDelegate<T>(T element);
}
