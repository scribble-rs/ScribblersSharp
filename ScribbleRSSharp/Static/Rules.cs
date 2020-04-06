/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Rules class
    /// </summary>
    public static class Rules
    {
        /// <summary>
        /// Minimal username length
        /// </summary>
        public static readonly uint minimalUsernameLength = 1U;

        /// <summary>
        /// Maximal username length
        /// </summary>
        public static readonly uint maximalUsernameLength = 30U;

        /// <summary>
        /// Minimal players
        /// </summary>
        public static readonly uint minimalPlayers = 2U;

        /// <summary>
        /// Maximal players
        /// </summary>
        public static readonly uint maximalPlayers = 24U;

        /// <summary>
        /// Minimal drawing time
        /// </summary>
        public static readonly uint minimalDrawingTime = 60U;

        /// <summary>
        /// Maximal drawing time
        /// </summary>
        public static readonly uint maximalDrawingTime = 300U;

        /// <summary>
        /// Minimal rounds
        /// </summary>
        public static readonly uint minimalRounds = 1U;

        /// <summary>
        /// Maximal rounds
        /// </summary>
        public static readonly uint maximalRounds = 20U;

        /// <summary>
        /// Minimal custom words chance
        /// </summary>
        public static readonly uint minimalCustomWordsChance = 1U;

        /// <summary>
        /// Maximal custom words chance
        /// </summary>
        public static readonly uint maximalCustomWordsChance = 100U;

        /// <summary>
        /// Minimal clients per IP limit
        /// </summary>
        public static readonly uint minimalClientsPerIPLimit = 1U;

        /// <summary>
        /// Maximal clients per IP limit
        /// </summary>
        public static readonly uint maximalClientsPerIPLimit = 24U;
    }
}
