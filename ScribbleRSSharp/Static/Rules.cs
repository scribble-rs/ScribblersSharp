/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    public static class Rules
    {
        public static readonly uint minimalUsernameLength = 1U;
        public static readonly uint maximalUsernameLength = 30U;
        public static readonly uint minimalPlayers = 2U;
        public static readonly uint maximalPlayers = 24U;
        public static readonly uint minimalDrawingTime = 60U;
        public static readonly uint maximalDrawingTime = 300U;
        public static readonly uint minimalRounds = 1U;
        public static readonly uint maximalRounds = 20U;
        public static readonly uint minimalCustomWordsChance = 1U;
        public static readonly uint maximalCustomWordsChance = 100U;
        public static readonly uint minimalClientsPerIPLimit = 1U;
        public static readonly uint maximalClientsPerIPLimit = 24U;
    }
}
