using System.Collections.Generic;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents lobby views
    /// </summary>
    public interface ILobbyViews : IEnumerable<ILobbyView>
    {
        /// <summary>
        /// Is connection secure
        /// </summary>
        bool IsConnectionSecure { get; }
    }
}
