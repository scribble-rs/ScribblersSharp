using System;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A structure that describes lobby limits
    /// </summary>
    internal readonly struct LobbyLimits : ILobbyLimits
    {
        /// <summary>
        /// Minimal drawing time in seconds
        /// </summary>
        public uint MinimalDrawingTime { get; }

        /// <summary>
        /// Maximal drawing time in seconds
        /// </summary>
        public uint MaximalDrawingTime { get; }

        /// <summary>
        /// Minimal round count
        /// </summary>
        public uint MinimalRoundCount { get; }

        /// <summary>
        /// Maximal round count
        /// </summary>
        public uint MaximalRoundCount { get; }

        /// <summary>
        /// Minimal of maximal player count
        /// </summary>
        public uint MinimalMaximalPlayerCount { get; }

        /// <summary>
        /// Maximal of maximal player count
        /// </summary>
        public uint MaximalMaximalPlayerCount { get; }

        /// <summary>
        /// Minimal allowed clients per IP count
        /// </summary>
        public uint MinimalAllowedClientsPerIPCount { get; }

        /// <summary>
        /// Maximal allowed clients per IP count
        /// </summary>
        public uint MaximalAllowedClientsPerIPCount { get; }

        /// <summary>
        /// Minimal brush size
        /// </summary>
        public uint MinimalBrushSize { get; }

        /// <summary>
        /// Maximal brush size
        /// </summary>
        public uint MaximalBrushSize { get; }

        /// <summary>
        /// Constructs lobby settings limits
        /// </summary>
        /// <param name="minimalDrawingTime">Minimal drawing time in seconds</param>
        /// <param name="maximalDrawingTime">Maximal drawing time in seconds</param>
        /// <param name="minimalRoundCount">Minimal round count</param>
        /// <param name="maximalRoundCount">Maximal round count</param>
        /// <param name="minimalMaximalPlayerCount">Minimal of maximal player count</param>
        /// <param name="maximalMaximalPlayerCount">Maximal of maximal player count</param>
        /// <param name="minimalAllowedClientsPerIPCount">Minimal allowed clients per IP count</param>
        /// <param name="maximalAllowedClientsPerIPCount">Maximal allowed clients per IP count</param>
        /// <param name="minimalBrushSize">Minimal brush size</param>
        /// <param name="maximalBrushSize">Maximal brush size</param>
        public LobbyLimits
        (
            uint minimalDrawingTime,
            uint maximalDrawingTime,
            uint minimalRoundCount,
            uint maximalRoundCount,
            uint minimalMaximalPlayerCount,
            uint maximalMaximalPlayerCount,
            uint minimalAllowedClientsPerIPCount,
            uint maximalAllowedClientsPerIPCount,
            uint minimalBrushSize,
            uint maximalBrushSize
        )
        {
            if (minimalDrawingTime < 1U)
            {
                throw new ArgumentException("Minimal drawing time can't be smaller than one.", nameof(minimalDrawingTime));
            }
            if (maximalDrawingTime < minimalDrawingTime)
            {
                throw new ArgumentException("Maximal drawing time can't be smaller than minimal drawing time.", nameof(maximalDrawingTime));
            }
            if (minimalRoundCount < 1U)
            {
                throw new ArgumentException("Minimal round count can't be smaller than one.", nameof(minimalRoundCount));
            }
            if (maximalRoundCount < minimalRoundCount)
            {
                throw new ArgumentException("Maximal round count can't be smaller than minimal round count.", nameof(maximalRoundCount));
            }
            if (minimalMaximalPlayerCount < 1U)
            {
                throw new ArgumentException("Minimal of maximal player count can't be smaller than one.", nameof(minimalMaximalPlayerCount));
            }
            if (maximalMaximalPlayerCount < minimalMaximalPlayerCount)
            {
                throw new ArgumentException("Maximal of maximal player count can't be smaller than minimal of maximal player count.", nameof(maximalMaximalPlayerCount));
            }
            if (minimalAllowedClientsPerIPCount < 1U)
            {
                throw new ArgumentException("Minimal clients per IP limit can't be smaller than one.", nameof(minimalAllowedClientsPerIPCount));
            }
            if (maximalAllowedClientsPerIPCount < minimalAllowedClientsPerIPCount)
            {
                throw new ArgumentException("Maximal clients per IP limit can't be smaller than minimal clients per IP limit.", nameof(maximalAllowedClientsPerIPCount));
            }
            if (minimalBrushSize < 1U)
            {
                throw new ArgumentException("Minimal brush size can't be smaller than one.", nameof(minimalBrushSize));
            }
            if (maximalBrushSize < minimalBrushSize)
            {
                throw new ArgumentException("Maximal brush size can't be smaller than maximal brush size.", nameof(maximalBrushSize));
            }
            MinimalDrawingTime = minimalDrawingTime;
            MaximalDrawingTime = maximalDrawingTime;
            MinimalRoundCount = minimalRoundCount;
            MaximalRoundCount = maximalRoundCount;
            MinimalMaximalPlayerCount = minimalMaximalPlayerCount;
            MaximalMaximalPlayerCount = maximalMaximalPlayerCount;
            MinimalAllowedClientsPerIPCount = minimalAllowedClientsPerIPCount;
            MaximalAllowedClientsPerIPCount = maximalAllowedClientsPerIPCount;
            MinimalBrushSize = minimalBrushSize;
            MaximalBrushSize = maximalBrushSize;
        }

        /// <summary>
        /// Checks if the given value is valid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimal"></param>
        /// <param name="maximal"></param>
        /// <returns></returns>
        private bool IsValid(uint value, uint minimal, uint maximal) => (value >= minimal) && (value <= maximal);

        /// <summary>
        /// Checks if the given drawing time is valid
        /// </summary>
        /// <param name="drawingTime">Drawing time</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public bool IsDrawingTimeValid(uint drawingTime) => IsValid(drawingTime, MinimalDrawingTime, MaximalDrawingTime);

        /// <summary>
        /// Checks if the given round count is valid
        /// </summary>
        /// <param name="roundCount">Round count</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public bool IsRoundCountValid(uint roundCount) => IsValid(roundCount, MinimalRoundCount, MaximalRoundCount);

        /// <summary>
        /// Checks if the given maximal player count is valid
        /// </summary>
        /// <param name="maximalPlayerCount">Maximal player count</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public bool IsMaximalPlayerCountValid(uint maximalPlayerCount) => IsValid(maximalPlayerCount, MinimalMaximalPlayerCount, MaximalMaximalPlayerCount);

        /// <summary>
        /// Checks if the given allowed clients per IP count is valid
        /// </summary>
        /// <param name="allowedClientsPerIPCount">Allowed clients per IP</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public bool IsAllowedClientsPerIPCountValid(uint allowedClientsPerIPCount) => IsValid(allowedClientsPerIPCount, MinimalAllowedClientsPerIPCount, MaximalAllowedClientsPerIPCount);

        /// <summary>
        /// Checks if the given brush size is valid
        /// </summary>
        /// <param name="brushSize">Brush size</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public bool IsBrushSizeAllowed(uint brushSize) => IsValid(brushSize, MinimalBrushSize, MaximalBrushSize);
    }
}
