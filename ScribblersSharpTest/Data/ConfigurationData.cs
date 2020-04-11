using System.Text.Json.Serialization;

/// <summary>
/// Scribble.rs ♯ test data namespace
/// </summary>
namespace ScribblersSharpTest.Data
{
    /// <summary>
    /// Configuration data class
    /// </summary>
    internal class ConfigurationData
    {
        /// <summary>
        /// Host
        /// </summary>
        [JsonPropertyName("host")]
        public string Host { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        [JsonPropertyName("port")]
        public ushort Port { get; set; }
    }
}
