using System;
using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # data namespace
/// </summary>
namespace ScribbleRSSharp.Data
{
    /// <summary>
    /// Enter lobby request data class
    /// </summary>
    internal class EnterLobbyRequestData : IRequestData<EnterLobbyResponseData>
    {
        /// <summary>
        /// UUID
        /// </summary>
        [JsonPropertyName("uuid")]
        public string UUID { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uuid">UUID</param>
        /// <param name="username">Username</param>
        public EnterLobbyRequestData(string uuid, string username)
        {
            if (uuid == null)
            {
                throw new ArgumentNullException(nameof(uuid));
            }
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            UUID = uuid;
            Username = username;
        }
    }
}
