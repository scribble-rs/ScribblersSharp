using System;
using System.Collections.Generic;

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
        /// Receive game message data name
        /// </summary>
        private static readonly string receiveGameMessageDataName = "ReceiveGameMessageData";

        /// <summary>
        /// Send game message data name
        /// </summary>
        private static readonly string sendGameMessageDataName = "SendGameMessageData";

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

        /// <summary>
        /// Converts the specified input string to kebab case
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Specified input string in kebab case</returns>
        public static string ConvertToKebabCase(string input)
        {
            List<char> result = new List<char>();
            string trimmed_input = input.Trim();
            bool is_first = true;
            bool previous_is_not_upper_case = true;
            foreach (char input_character in trimmed_input)
            {
                if (is_first)
                {
                    is_first = false;
                }
                else if (char.IsUpper(input_character))
                {
                    if (previous_is_not_upper_case)
                    {
                        result.Add('-');
                    }
                    previous_is_not_upper_case = false;
                }
                else
                {
                    previous_is_not_upper_case = true;
                }
                result.Add(char.ToLower(input_character));
            }
            string ret = new string(result.ToArray());
            result.Clear();
            return ret;
        }

        /// <summary>
        /// Gets the name for the specified receive game message data class in kebab case
        /// </summary>
        /// <typeparam name="T">Receive game message data class</typeparam>
        /// <returns>Specified receive game message data class name in kebab case</returns>
        public static string GetReceiveGameMessageDataNameInKebabCase<T>() where T : IReceiveGameMessageData => GetTypeNameWithoutSuffixInKebabCase<T>(receiveGameMessageDataName);

        /// <summary>
        /// Gets the name for the specified send game message data class in kebab case
        /// </summary>
        /// <typeparam name="T">Send game message data class</typeparam>
        /// <returns>Specified send game message data class name in kebab case</returns>
        public static string GetSendGameMessageDataNameInKebabCase<T>() where T : ISendGameMessageData => GetTypeNameWithoutSuffixInKebabCase<T>(sendGameMessageDataName);

        /// <summary>
        /// Gets the type name without its suffix in kebab case
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="suffix">Suffix</param>
        /// <returns>Specified type name in kebab case</returns>
        public static string GetTypeNameWithoutSuffixInKebabCase<T>(string suffix)
        {
            if (suffix == null)
            {
                throw new ArgumentNullException(nameof(suffix));
            }
            string type_name = typeof(T).Name;
            return ConvertToKebabCase(type_name.EndsWith(suffix) ? type_name.Substring(0, type_name.Length - suffix.Length) : type_name);
        }
    }
}
