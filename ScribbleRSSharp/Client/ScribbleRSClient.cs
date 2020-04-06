using ScribblersSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// scribble.rs client
    /// </summary>
    public class ScribbleRSClient : IDisposable
    {
        /// <summary>
        /// HTTP client
        /// </summary>
        private HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Post HTTP (asynchronous)
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="requestURI">Request URI</param>
        /// <param name="request">Request</param>
        /// <returns>Response if successful, otherwise "null"</returns>
        private async Task<T> PostHTTPAsync<T>(Uri requestURI, IRequestData<T> request) where T : IResponseData
        {
            T ret = default;
            using (MemoryStream memory_stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(memory_stream, request);
                memory_stream.Seek(0L, SeekOrigin.Begin);
                using (StreamContent stream_content = new StreamContent(memory_stream))
                {
                    using (HttpResponseMessage response = await httpClient.PostAsync(requestURI, stream_content))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            ret = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync());
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Enter lobby (asynchronous)
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="uuid">UUID</param>
        /// <param name="username">Username</param>
        /// <returns>Lobby task</returns>
        public async Task<ILobby> EnterLobbyAsync(string host, string uuid, string username)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (uuid == null)
            {
                throw new ArgumentNullException(nameof(uuid));
            }
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            if ((username.Length < Rules.minimalUsernameLength) || (username.Length > Rules.maximalUsernameLength))
            {
                throw new ArgumentException("Username must be between " + Rules.minimalUsernameLength + " and " + Rules.maximalUsernameLength + " characters.");
            }
            ILobby ret = null;
            Uri http_host_uri = new Uri("https://" + host);
            Uri web_socket_host_uri = new Uri("wss://" + host);
            EnterLobbyResponseData response = await PostHTTPAsync(new Uri(http_host_uri, "/v1/enterLobby"), new EnterLobbyRequestData(uuid, username));
            if (response != null)
            {
                ClientWebSocket client_web_socket = new ClientWebSocket();
                await client_web_socket.ConnectAsync(web_socket_host_uri, default);
                if (client_web_socket.State == WebSocketState.Open)
                {
                    ret = new Lobby(client_web_socket, username, response.LobbyID, response.DrawingBoardBaseWidth, response.DrawingBoardBaseHeight);
                }
                else
                {
                    client_web_socket.Dispose();
                }
            }
            return ret;
        }

        /// <summary>
        /// Create lobby (asynchronous)
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="username">Username</param>
        /// <param name="language">Language</param>
        /// <param name="maximalPlayers">Maximal players</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="rounds">Rounds</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="enableVotekick">Enable vote kick</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        /// <returns>Lobby task</returns>
        public async Task<ILobby> CreateLobbyAsync(string host, string username, ELanguage language, uint maximalPlayers, ulong drawingTime, uint rounds, IReadOnlyList<string> customWords, uint customWordsChance, bool enableVotekick, uint clientsPerIPLimit)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            if ((username.Length < Rules.minimalUsernameLength) || (username.Length > Rules.maximalUsernameLength))
            {
                throw new ArgumentException("Username must be between " + Rules.minimalUsernameLength + " and " + Rules.maximalUsernameLength + " characters.");
            }
            if ((maximalPlayers < Rules.minimalPlayers) || (maximalPlayers > Rules.maximalPlayers))
            {
                throw new ArgumentException("There can be only " + Rules.minimalPlayers + " to " + Rules.maximalPlayers + " players in one lobby.");
            }
            if ((drawingTime < Rules.minimalDrawingTime) || (drawingTime > Rules.maximalDrawingTime))
            {
                throw new ArgumentException("Drawing time can only be between " + Rules.minimalDrawingTime + " and " + Rules.maximalDrawingTime + " seconds.");
            }
            if ((rounds < Rules.minimalRounds) || (rounds > Rules.maximalRounds))
            {
                throw new ArgumentException("Only " + Rules.minimalRounds + " to " + Rules.maximalRounds + " rounds can be set in a lobby.");
            }
            if (customWords == null)
            {
                throw new ArgumentNullException(nameof(customWords));
            }
            if ((customWordsChance < Rules.minimalCustomWordsChance) || (customWordsChance > Rules.maximalCustomWordsChance))
            {
                throw new ArgumentException("Custom words chance must be between " + Rules.minimalCustomWordsChance + " and " + Rules.maximalCustomWordsChance + ".");
            }
            if ((clientsPerIPLimit < Rules.minimalClientsPerIPLimit) || (customWordsChance > Rules.maximalClientsPerIPLimit))
            {
                throw new ArgumentException("Clients per IP limit must be between " + Rules.minimalClientsPerIPLimit + " and " + Rules.maximalClientsPerIPLimit + ".");
            }
            ILobby ret = null;
            Uri http_host_uri = new Uri("https://" + host);
            Uri web_socket_host_uri = new Uri("wss://" + host);
            string[] custom_words = new string[customWords.Count];
            Parallel.For(0, custom_words.Length, (index) =>
            {
                string custom_word = customWords[index];
                if (custom_word == null)
                {
                    throw new ArgumentNullException(nameof(custom_word));
                }
                custom_words[index] = custom_word;
            });
            CreateLobbyResponseData response = await PostHTTPAsync(new Uri(http_host_uri, "/v1/lobby"), new CreateLobbyRequestData(username, language, maximalPlayers, drawingTime, rounds, custom_words, customWordsChance, enableVotekick, clientsPerIPLimit));
            if (response != null)
            {
                ClientWebSocket client_web_socket = new ClientWebSocket();
                await client_web_socket.ConnectAsync(web_socket_host_uri, default);
                if (client_web_socket.State == WebSocketState.Open)
                {
                    ret = new Lobby(client_web_socket, username, response.LobbyID, response.DrawingBoardBaseWidth, response.DrawingBoardBaseHeight);
                }
                else
                {
                    client_web_socket.Dispose();
                }
            }
            return ret;
        }

        /// <summary>
        /// Dispose scribble.rs client
        /// </summary>
        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
