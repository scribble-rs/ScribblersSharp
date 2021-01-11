/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Are unique delegate
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="left">Left hand element</param>
    /// <param name="right">RIght hand element</param>
    /// <returns>"true" if both elements are unique, otherwise "false"</returns>
    internal delegate bool AreUniqueDelegate<T>(T left, T right);
}
