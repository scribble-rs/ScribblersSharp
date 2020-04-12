using Newtonsoft.Json;
using ScribblersSharp.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Scribble.rs client
    /// </summary>
    public class ScribblersClient : IDisposable
    {
        /// <summary>
        /// HTTP protocol
        /// </summary>
        private static readonly string httpProtocol = "http";

        /// <summary>
        /// WebSocket protocol
        /// </summary>
        private static readonly string webSocketProtocol = "ws";

        /// <summary>
        /// Cookie container
        /// </summary>
        private CookieContainer cookieContainer = new CookieContainer();

        /// <summary>
        /// HTTP client
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScribblersClient()
        {
            httpClient = new HttpClient(new HttpClientHandler { UseCookies = true, CookieContainer = cookieContainer });
            httpClient.Timeout = TimeSpan.FromSeconds(3000.0);
        }

        /// <summary>
        /// Post HTTP (asynchronous)
        /// </summary>
        /// <param name="requestURI">Request URI</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Response if successful, otherwise "null"</returns>
        private async Task<ResponseWithUserSessionCookie<T>> PostHTTPAsync<T>(Uri requestURI, IReadOnlyDictionary<string, string> parameters) where T : IResponseData
        {
            ResponseWithUserSessionCookie<T> ret = default;
            string user_session_cookie = string.Empty;
            using (HttpResponseMessage response = await httpClient.PostAsync(requestURI, new FormUrlEncodedContent(parameters)))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ret = new ResponseWithUserSessionCookie<T>(JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()), user_session_cookie);
                }
                else
                {
                    Console.Error.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
            return ret;
        }

        /// <summary>
        /// Enter lobby (asynchronous)
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="lobbyID">Lobby ID</param>
        /// <param name="username">Username</param>
        /// <returns>Lobby task</returns>
        public async Task<ILobby> EnterLobbyAsync(string host, string lobbyID, string username)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (lobbyID == null)
            {
                throw new ArgumentNullException(nameof(lobbyID));
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
            Uri http_host_uri = new Uri(httpProtocol + "://" + host);
            Uri web_socket_host_uri = new Uri(webSocketProtocol + "://" + host);
            ResponseWithUserSessionCookie<EnterLobbyResponseData> response_with_user_session_cookie = await PostHTTPAsync<EnterLobbyResponseData>(new Uri(http_host_uri, "/v1/lobby/player?lobby_id=" + Uri.EscapeUriString(lobbyID)), new Dictionary<string, string>
            {
                { "lobby_id", lobbyID },
                { "username", username }
            });
            EnterLobbyResponseData response = response_with_user_session_cookie.Response;
            if (response != null)
            {
                ClientWebSocket client_web_socket = new ClientWebSocket();
                client_web_socket.Options.Cookies = cookieContainer;
                await client_web_socket.ConnectAsync(new Uri(web_socket_host_uri, "/v1/ws?lobby_id=" + Uri.EscapeUriString(response.LobbyID)), default);
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
            if ((clientsPerIPLimit < Rules.minimalClientsPerIPLimit) || (clientsPerIPLimit > Rules.maximalClientsPerIPLimit))
            {
                throw new ArgumentException("Clients per IP limit must be between " + Rules.minimalClientsPerIPLimit + " and " + Rules.maximalClientsPerIPLimit + ".");
            }
            ILobby ret = null;
            Uri http_host_uri = new Uri(httpProtocol + "://" + host);
            Uri web_socket_host_uri = new Uri(webSocketProtocol + "://" + host);
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
            StringBuilder custom_words_builder = new StringBuilder();
            bool first = true;
            foreach (string custom_word in customWords)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    custom_words_builder.Append(",");
                }
                custom_words_builder.Append(custom_word);
            }
            ResponseWithUserSessionCookie<CreateLobbyResponseData> response_with_user_session_cookie = await PostHTTPAsync<CreateLobbyResponseData>(new Uri(http_host_uri, "/v1/lobby"), new Dictionary<string, string>
            {
                { "username", username },
                { "language", language.ToString().ToLower() },
                { "max_players", maximalPlayers.ToString() },
                { "drawing_time", drawingTime.ToString() },
                { "rounds", rounds.ToString() },
                { "custom_words", custom_words_builder.ToString() },
                { "custom_words_chance", customWordsChance.ToString() },
                { "enable_votekick", enableVotekick.ToString() },
                { "clients_per_ip_limit", clientsPerIPLimit.ToString() }
            });
            custom_words_builder.Clear();
            CreateLobbyResponseData response = response_with_user_session_cookie.Response;
            if (response != null)
            {
                ClientWebSocket client_web_socket = new ClientWebSocket();
                client_web_socket.Options.Cookies = cookieContainer;
                await client_web_socket.ConnectAsync(new Uri(web_socket_host_uri, "/v1/ws?lobby_id=" + Uri.EscapeUriString(response.LobbyID)), default);
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
