/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// An interface that represents lobby limits
    /// </summary>
    public interface ILobbyLimits
    {
        /// <summary>
        /// Minimal drawing time in seconds
        /// </summary>
        uint MinimalDrawingTime { get; }

        /// <summary>
        /// Maximal drawing time in seconds
        /// </summary>
        uint MaximalDrawingTime { get; }

        /// <summary>
        /// Minimal round count
        /// </summary>
        uint MinimalRoundCount { get; }

        /// <summary>
        /// Maximal round count
        /// </summary>
        uint MaximalRoundCount { get; }

        /// <summary>
        /// Minimal of maximal player count
        /// </summary>
        uint MinimalMaximalPlayerCount { get; }

        /// <summary>
        /// Maximal of maximal player count
        /// </summary>
        uint MaximalMaximalPlayerCount { get; }

        /// <summary>
        /// Minimal allowed clients per IP count
        /// </summary>
        uint MinimalAllowedClientsPerIPCount { get; }

        /// <summary>
        /// Maximal allowed clients per IP count
        /// </summary>
        uint MaximalAllowedClientsPerIPCount { get; }

        /// <summary>
        /// Minimal brush size
        /// </summary>
        uint MinimalBrushSize { get; }

        /// <summary>
        /// Maximal brush size
        /// </summary>
        uint MaximalBrushSize { get; }

        /// <summary>
        /// Checks if the given drawing time is valid
        /// </summary>
        /// <param name="drawingTime">Drawing time</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        bool IsDrawingTimeValid(uint drawingTime);

        /// <summary>
        /// Checks if the given round count is valid
        /// </summary>
        /// <param name="roundCount">Round count</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        bool IsRoundCountValid(uint roundCount);

        /// <summary>
        /// Checks if the given maximal player count is valid
        /// </summary>
        /// <param name="maximalPlayerCount">Maximal player count</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        bool IsMaximalPlayerCountValid(uint maximalPlayerCount);

        /// <summary>
        /// Checks if the given allowed clients per IP count is valid
        /// </summary>
        /// <param name="allowedClientsPerIPCount">Allowed clients per IP</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        bool IsAllowedClientsPerIPCountValid(uint allowedClientsPerIPCount);

        /// <summary>
        /// Checks if the given brush size is valid
        /// </summary>
        /// <param name="brushSize">Brush size</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        bool IsBrushSizeAllowed(uint brushSize);
    }
}
