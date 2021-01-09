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
    /// A class that describes a Scribble.rs client
    /// </summary>
    internal class ScribblersClient : IScribblersClient
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
        private readonly CookieContainer cookieContainer = new CookieContainer();

        /// <summary>
        /// HTTP client
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Scribble.rs host
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Constructs a Scribble.rs client
        /// </summary>
        /// <param name="host">Scribble.rs host</param>
        public ScribblersClient(string host)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            httpClient = new HttpClient(new HttpClientHandler { UseCookies = true, CookieContainer = cookieContainer })
            {
                Timeout = TimeSpan.FromSeconds(3000.0)
            };
        }

        /// <summary>
        /// Posts a HTTP request (asynchronous)
        /// </summary>
        /// <param name="requestURI">Request URI</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Response if successful, otherwise "null"</returns>
        private async Task<ResponseWithUserSessionCookie<T>> PostHTTPAsync<T>(Uri requestURI, IReadOnlyDictionary<string, string> parameters) where T : IResponseData
        {
            ResponseWithUserSessionCookie<T> ret = default;
            string user_session_cookie = string.Empty;
            try
            {
                using (HttpResponseMessage response = await httpClient.PostAsync(requestURI, new FormUrlEncodedContent(parameters)))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        ret = new ResponseWithUserSessionCookie<T>(JsonConvert.DeserializeObject<T>(json), user_session_cookie);
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        Console.Error.WriteLine(error);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return ret;
        }

        /// <summary>
        /// Enters a lobby (asynchronous)
        /// </summary>
        /// <param name="lobbyID">Lobby ID</param>
        /// <param name="username">Username</param>
        /// <returns>Lobby task</returns>
        public async Task<ILobby> EnterLobbyAsync(string lobbyID, string username)
        {
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
                throw new ArgumentException($"Username must be between { Rules.minimalUsernameLength } and { Rules.maximalUsernameLength } characters.");
            }
            ILobby ret = null;
            Uri http_host_uri = new Uri($"{ httpProtocol }://{ Host }");
            Uri web_socket_host_uri = new Uri($"{ webSocketProtocol }://{ Host }");
            ResponseWithUserSessionCookie<EnterLobbyResponseData> response_with_user_session_cookie = await PostHTTPAsync<EnterLobbyResponseData>(new Uri(http_host_uri, $"/v1/lobby/player?lobby_id={ Uri.EscapeUriString(lobbyID) }"), new Dictionary<string, string>
            {
                { "lobby_id", lobbyID },
                { "username", username }
            });
            EnterLobbyResponseData response = response_with_user_session_cookie.Response;
            if (response != null)
            {
                ClientWebSocket client_web_socket = new ClientWebSocket();
                client_web_socket.Options.Cookies = cookieContainer;
                await client_web_socket.ConnectAsync(new Uri(web_socket_host_uri, $"/v1/ws?lobby_id={ Uri.EscapeUriString(response.LobbyID) }"), default);
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
        /// Creates a new lobby (asynchronous)
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="language">Language</param>
        /// <param name="isPublic">Is lobby public</param>
        /// <param name="maximalPlayers">Maximal players</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="rounds">Rounds</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="enableVotekick">Enable vote kick</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        /// <returns>Lobby task</returns>
        public async Task<ILobby> CreateLobbyAsync(string username, ELanguage language, bool isPublic, uint maximalPlayers, ulong drawingTime, uint rounds, IReadOnlyList<string> customWords, uint customWordsChance, bool enableVotekick, uint clientsPerIPLimit)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            if ((username.Length < Rules.minimalUsernameLength) || (username.Length > Rules.maximalUsernameLength))
            {
                throw new ArgumentException($"Username must be between { Rules.minimalUsernameLength } and { Rules.maximalUsernameLength } characters.");
            }
            if ((maximalPlayers < Rules.minimalPlayers) || (maximalPlayers > Rules.maximalPlayers))
            {
                throw new ArgumentException($"There can be only { Rules.minimalPlayers } to { Rules.maximalPlayers } players in one lobby.");
            }
            if ((drawingTime < Rules.minimalDrawingTime) || (drawingTime > Rules.maximalDrawingTime))
            {
                throw new ArgumentException($"Drawing time can only be between { Rules.minimalDrawingTime } and { Rules.maximalDrawingTime } seconds.");
            }
            if ((rounds < Rules.minimalRounds) || (rounds > Rules.maximalRounds))
            {
                throw new ArgumentException($"Only { Rules.minimalRounds } to { Rules.maximalRounds } rounds can be set in a lobby.");
            }
            if (customWords == null)
            {
                throw new ArgumentNullException(nameof(customWords));
            }
            if ((customWordsChance < Rules.minimalCustomWordsChance) || (customWordsChance > Rules.maximalCustomWordsChance))
            {
                throw new ArgumentException($"Custom words chance must be between { Rules.minimalCustomWordsChance } and { Rules.maximalCustomWordsChance }.");
            }
            if ((clientsPerIPLimit < Rules.minimalClientsPerIPLimit) || (clientsPerIPLimit > Rules.maximalClientsPerIPLimit))
            {
                throw new ArgumentException($"Clients per IP limit must be between { Rules.minimalClientsPerIPLimit } and { Rules.maximalClientsPerIPLimit }.");
            }
            ILobby ret = null;
            Uri http_host_uri = new Uri($"{ httpProtocol }://{ Host }");
            Uri web_socket_host_uri = new Uri($"{ webSocketProtocol }://{ Host }");
            string[] custom_words = new string[customWords.Count];
            Parallel.For(0, custom_words.Length, (index) =>
            {
                string custom_word = customWords[index];
                custom_words[index] = custom_word ?? throw new ArgumentNullException(nameof(custom_word));
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
                { "public", isPublic.ToString().ToLower() },
                { "max_players", maximalPlayers.ToString() },
                { "drawing_time", drawingTime.ToString() },
                { "rounds", rounds.ToString() },
                { "custom_words", custom_words_builder.ToString() },
                { "custom_words_chance", customWordsChance.ToString() },
                { "enable_votekick", enableVotekick.ToString().ToLower() },
                { "clients_per_ip_limit", clientsPerIPLimit.ToString() }
            });
            custom_words_builder.Clear();
            CreateLobbyResponseData response = response_with_user_session_cookie.Response;
            if (response != null)
            {
                ClientWebSocket client_web_socket = new ClientWebSocket();
                client_web_socket.Options.Cookies = cookieContainer;
                await client_web_socket.ConnectAsync(new Uri(web_socket_host_uri, $"/v1/ws?lobby_id={ Uri.EscapeUriString(response.LobbyID) }"), default);
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
        /// Dispose Scribble.rs client
        /// </summary>
        public void Dispose() => httpClient.Dispose();
    }
}
