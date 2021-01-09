using System;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A class used for converting between naming conventions
    /// </summary>
    internal static class Naming
    {
        /// <summary>
        /// Uppers the first character of the specified string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Upper case variant of the specified input string</returns>
        public static string UpperFirstCharacter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }
            string ret = input.Trim();
            return char.ToUpper(ret[0]) + ret.Substring(1);
        }

        /// <summary>
        /// Lowers the first character of the specified string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Lower case variant of the specified input string</returns>
        public static string LowerFirstCharacter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }
            string ret = input.Trim();
            return char.ToLower(ret[0]) + ret.Substring(1);
        }
    }
}
