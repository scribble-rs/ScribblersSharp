using Newtonsoft.Json;
using ScribblersSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// Secure HTTP protocol
        /// </summary>
        private static readonly string secureHTTPProtocol = "https";

        /// <summary>
        /// WebSocket protocol
        /// </summary>
        private static readonly string webSocketProtocol = "ws";

        /// <summary>
        /// Secure WebSocket protocol
        /// </summary>
        private static readonly string secureWebSocketProtocol = "wss";

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
        /// Is using secure protocols
        /// </summary>
        public bool IsUsingSecureProtocols { get; }

        /// <summary>
        /// Constructs a Scribble.rs client
        /// </summary>
        /// <param name="host">Scribble.rs host</param>
        public ScribblersClient(string host, bool isUsingSecureProtocols)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            IsUsingSecureProtocols = isUsingSecureProtocols;
            httpClient = new HttpClient(new HttpClientHandler { UseCookies = true, CookieContainer = cookieContainer })
            {
                Timeout = TimeSpan.FromSeconds(3000.0)
            };
        }

        /// <summary>
        /// Sends a HTTP GET request asynchronously
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="requestURI">Request URI</param>
        /// <returns>Response if successful, otherwise "default"</returns>
        private async Task<T> SendHTTPGETRequestAsync<T>(Uri requestURI)
        {
            T ret = default;
            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(requestURI))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ret = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        Console.Error.WriteLine(await response.Content.ReadAsStringAsync());
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
        /// Sends a HTTP POST request asynchronously
        /// </summary>
        /// <param name="requestURI">Request URI</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Response if successful, otherwise "default"</returns>
        private async Task<ResponseWithUserSessionCookie<T>> SendHTTPPostRequestAsync<T>(Uri requestURI, IReadOnlyDictionary<string, string> parameters) where T : IResponseData
        {
            ResponseWithUserSessionCookie<T> ret = default;
            string user_session_cookie = string.Empty;
            try
            {
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
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return ret;
        }

        /// <summary>
        /// Sends a HTTP patch request asynchronously
        /// </summary>
        /// <param name="requestURI">Request URI</param>
        /// <returns>Task</returns>
        public async Task SendHTTPPATCHAsync(Uri requestURI)
        {
            try
            {
                using (_ = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestURI)))
                {
                    // ...
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        /// <summary>
        /// Enters a lobby asynchronously
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
            Uri http_host_uri = new Uri($"{ (IsUsingSecureProtocols ? secureHTTPProtocol : httpProtocol) }://{ Host }");
            Uri web_socket_host_uri = new Uri($"{ (IsUsingSecureProtocols ? secureWebSocketProtocol : webSocketProtocol) }://{ Host }");
            ResponseWithUserSessionCookie<EnterLobbyResponseData> response_with_user_session_cookie = await SendHTTPPostRequestAsync<EnterLobbyResponseData>(new Uri(http_host_uri, $"/v1/lobby/player?lobby_id={ Uri.EscapeUriString(lobbyID) }"), new Dictionary<string, string>
            {
                { "lobby_id", lobbyID },
                { "username", username }
            });
            EnterLobbyResponseData response = response_with_user_session_cookie.Response;
            if ((response != null) && response.IsValid)
            {
                ClientWebSocket client_web_socket = new ClientWebSocket();
                client_web_socket.Options.Cookies = cookieContainer;
                await client_web_socket.ConnectAsync(new Uri(web_socket_host_uri, $"/v1/ws?lobby_id={ Uri.EscapeUriString(response.LobbyID) }"), default);
                if (client_web_socket.State == WebSocketState.Open)
                {
                    ret = new Lobby
                    (
                        client_web_socket,
                        response.LobbyID,
                        response.MinimalDrawingTime,
                        response.MaximalDrawingTime,
                        response.MinimalRoundCount,
                        response.MaximalRoundCount,
                        response.MinimalMaximalPlayerCount,
                        response.MaximalMaximalPlayerCount,
                        response.MinimalClientsPerIPLimit,
                        response.MaximalClientsPerIPLimit,
                        response.MaximalPlayerCount,
                        response.IsPublic,
                        response.IsVotekickingEnabled,
                        response.CustomWordsChance,
                        response.ClientsPerIPLimit,
                        response.DrawingBoardBaseWidth,
                        response.DrawingBoardBaseHeight,
                        response.MinimalBrushSize,
                        response.MaximalBrushSize,
                        response.SuggestedBrushSizes,
                        Color.FromArgb(0xFF, response.CanvasColor[0], response.CanvasColor[1], response.CanvasColor[2])
                    );
                }
                else
                {
                    client_web_socket.Dispose();
                }
            }
            return ret;
        }

        /// <summary>
        /// Creates a new lobby asynchronously
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="language">Language</param>
        /// <param name="isPublic">Is lobby public</param>
        /// <param name="maximalPlayerCount">Maximal player count</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="roundCount">Round count</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="isVotekickingEnabled">Is votekicking enabled</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        /// <returns>Lobby task</returns>
        public async Task<ILobby> CreateLobbyAsync(string username, ELanguage language, bool isPublic, uint maximalPlayerCount, ulong drawingTime, uint roundCount, IReadOnlyList<string> customWords, uint customWordsChance, bool isVotekickingEnabled, uint clientsPerIPLimit)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (username.Length > Rules.maximalUsernameLength)
            {
                throw new ArgumentException($"Username must be atleast { Rules.maximalUsernameLength } characters long.");
            }
            if ((maximalPlayerCount < Rules.minimalPlayers) || (maximalPlayerCount > Rules.maximalPlayers))
            {
                throw new ArgumentException($"There can be only { Rules.minimalPlayers } to { Rules.maximalPlayers } players in one lobby.");
            }
            if ((drawingTime < Rules.minimalDrawingTime) || (drawingTime > Rules.maximalDrawingTime))
            {
                throw new ArgumentException($"Drawing time can only be between { Rules.minimalDrawingTime } and { Rules.maximalDrawingTime } seconds.");
            }
            if ((roundCount < Rules.minimalRounds) || (roundCount > Rules.maximalRounds))
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
            Uri http_host_uri = new Uri($"{ (IsUsingSecureProtocols ? secureHTTPProtocol : httpProtocol) }://{ Host }");
            Uri web_socket_host_uri = new Uri($"{ (IsUsingSecureProtocols ? secureWebSocketProtocol : webSocketProtocol) }://{ Host }");
            string[] custom_words = new string[customWords.Count];
#if SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
            for (int index = 0; index < custom_words.Length; index++)
#else
            Parallel.For(0, custom_words.Length, (index) =>
#endif
            {
                string custom_word = customWords[index];
                custom_words[index] = custom_word ?? throw new ArgumentNullException(nameof(custom_word));
            }
#if !SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
            );
#endif
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
            ResponseWithUserSessionCookie<CreateLobbyResponseData> response_with_user_session_cookie = await SendHTTPPostRequestAsync<CreateLobbyResponseData>(new Uri(http_host_uri, "/v1/lobby"), new Dictionary<string, string>
            {
                { "username", username },
                { "language", Naming.GetLanguageString(language) },
                { "public", isPublic.ToString().ToLower() },
                { "max_players", maximalPlayerCount.ToString() },
                { "drawing_time", drawingTime.ToString() },
                { "rounds", roundCount.ToString() },
                { "custom_words", custom_words_builder.ToString() },
                { "custom_words_chance", customWordsChance.ToString() },
                { "enable_votekick", isVotekickingEnabled.ToString().ToLower() },
                { "clients_per_ip_limit", clientsPerIPLimit.ToString() }
            });
            custom_words_builder.Clear();
            CreateLobbyResponseData response = response_with_user_session_cookie.Response;
            if ((response != null) && response.IsValid)
            {
                ClientWebSocket client_web_socket = new ClientWebSocket();
                client_web_socket.Options.Cookies = cookieContainer;
                await client_web_socket.ConnectAsync(new Uri(web_socket_host_uri, $"/v1/ws?lobby_id={ Uri.EscapeUriString(response.LobbyID) }"), default);
                if (client_web_socket.State == WebSocketState.Open)
                {
                    ret = new Lobby
                    (
                        client_web_socket,
                        response.LobbyID,
                        response.MinimalDrawingTime,
                        response.MaximalDrawingTime,
                        response.MinimalRoundCount,
                        response.MaximalRoundCount,
                        response.MinimalMaximalPlayerCount,
                        response.MaximalMaximalPlayerCount,
                        response.MinimalClientsPerIPLimit,
                        response.MaximalClientsPerIPLimit,
                        response.MaximalPlayerCount,
                        response.IsPublic,
                        response.IsVotekickingEnabled,
                        response.CustomWordsChance,
                        response.ClientsPerIPLimit,
                        response.DrawingBoardBaseWidth,
                        response.DrawingBoardBaseHeight,
                        response.MinimalBrushSize,
                        response.MaximalBrushSize,
                        response.SuggestedBrushSizes,
                        Color.FromArgb(0xFF, response.CanvasColor[0], response.CanvasColor[1], response.CanvasColor[2])
                    );
                }
                else
                {
                    client_web_socket.Dispose();
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets server statistics asynchronously
        /// </summary>
        /// <returns>Server statistics task</returns>
        public async Task<IServerStatistics> GetServerStatisticsAsync()
        {
            Uri http_host_uri = new Uri($"{ (IsUsingSecureProtocols ? secureHTTPProtocol : httpProtocol) }://{ Host }");
            ServerStatisticsData server_statistics = await SendHTTPGETRequestAsync<ServerStatisticsData>(new Uri(http_host_uri, "/v1/stats"));
            return (server_statistics == null) ? (IServerStatistics)null : new ServerStatistics(server_statistics.ActiveLobbyCount, server_statistics.PlayerCount, server_statistics.OccupiedPlayerSlotCount, server_statistics.ConnectedPlayerCount);
        }

        /// <summary>
        /// Lists all public lobbies asynchronously
        /// </summary>
        /// <returns>Lobby views task</returns>
        public async Task<IEnumerable<ILobbyView>> ListLobbiesAsync()
        {
            ILobbyView[] ret = Array.Empty<ILobbyView>();
            Uri http_host_uri = new Uri($"{ (IsUsingSecureProtocols ? secureHTTPProtocol : httpProtocol) }://{ Host }");
            LobbyViewData[] lobby_views = await SendHTTPGETRequestAsync<LobbyViewData[]>(new Uri(http_host_uri, "/v1/lobby"));
            if ((lobby_views != null) && Protection.IsValid(lobby_views))
            {
                ret = new ILobbyView[lobby_views.Length];
#if SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
                for (int lobby_view_index = 0; lobby_view_index < lobby_views.Length; lobby_view_index++)
#else
                Parallel.For(0, lobby_views.Length, (lobby_view_index) =>
#endif
                {
                    LobbyViewData lobby_view = lobby_views[lobby_view_index];
                    ret[lobby_view_index] = new LobbyView(lobby_view.LobbyID, lobby_view.PlayerCount, lobby_view.MaximalPlayerCount, lobby_view.RoundCount, lobby_view.MaximalRoundCount, lobby_view.DrawingTime, lobby_view.IsUsingCustomWords, lobby_view.IsVotekickingEnabled, lobby_view.MaximalClientsPerIPCount, lobby_view.Language);
                }
#if !SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
                );
#endif
            }
            return ret;
        }

        /// <summary>
        /// Changes lobby rules asynchronously
        /// </summary>
        /// <param name="language">Language (optional)</param>
        /// <param name="isPublic">Is lobby public (optional)</param>
        /// <param name="maximalPlayerCount">Maximal player count (optional)</param>
        /// <param name="drawingTime">Drawing time (optional)</param>
        /// <param name="roundCount">Round count (optional)</param>
        /// <param name="customWords">Custom words (optional)</param>
        /// <param name="customWordsChance">Custom words chance (optional)</param>
        /// <param name="isVotekickingEnabled">Is votekicking enabled (optional)</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit (optional)</param>
        /// <returns>Task</returns>
        public async Task ChangeLobbyRulesAsync(ELanguage? language = null, bool? isPublic = null, uint? maximalPlayerCount = null, ulong? drawingTime = null, uint? roundCount = null, IReadOnlyList<string> customWords = null, uint? customWordsChance = null, bool? isVotekickingEnabled = null, uint? clientsPerIPLimit = null)
        {
            if ((language != null) && (language == ELanguage.Invalid))
            {
                throw new ArgumentException("Language can't be invalid.", nameof(language));
            }
            if ((maximalPlayerCount != null) && (maximalPlayerCount < 1U))
            {
                throw new ArgumentException("Maximal player count can't be smaller than one.", nameof(maximalPlayerCount));
            }
            if ((drawingTime != null) && (drawingTime < 1U))
            {
                throw new ArgumentException("Drawing time can't be smaller than one.", nameof(drawingTime));
            }
            if ((customWords != null) && !Protection.IsValid(customWords))
            {
                throw new ArgumentException("Custom words can't contain null.", nameof(customWords));
            }
            if ((clientsPerIPLimit != null) && (clientsPerIPLimit < 1U))
            {
                throw new ArgumentException("Clients per IP limit can't be smaller than one.", nameof(clientsPerIPLimit));
            }
            Uri http_host_uri = new Uri($"{ (IsUsingSecureProtocols ? secureHTTPProtocol : httpProtocol) }://{ Host }");
            bool are_changes_specified = false;
            StringBuilder parameters_string_builder = new StringBuilder();
            if ((language != null) && (language != ELanguage.Invalid))
            {
                are_changes_specified = true;
                parameters_string_builder.Append($"language={ Uri.EscapeUriString(Naming.GetLanguageString(language.Value)) }");
            }
            if (isPublic != null)
            {
                if (are_changes_specified)
                {
                    parameters_string_builder.Append("&");
                }
                else
                {
                    are_changes_specified = true;
                }
                parameters_string_builder.Append($"public={ Uri.EscapeUriString(isPublic.Value.ToString().ToLower()) }");
            }
            if (maximalPlayerCount != null)
            {
                if (are_changes_specified)
                {
                    parameters_string_builder.Append("&");
                }
                else
                {
                    are_changes_specified = true;
                }
                parameters_string_builder.Append($"max_players={ Uri.EscapeUriString(maximalPlayerCount.Value.ToString()) }");
            }
            if (drawingTime != null)
            {
                if (are_changes_specified)
                {
                    parameters_string_builder.Append("&");
                }
                else
                {
                    are_changes_specified = true;
                }
                parameters_string_builder.Append($"drawing_time={ Uri.EscapeUriString(drawingTime.Value.ToString()) }");
            }
            if (roundCount != null)
            {
                if (are_changes_specified)
                {
                    parameters_string_builder.Append("&");
                }
                else
                {
                    are_changes_specified = true;
                }
                parameters_string_builder.Append($"rounds={ Uri.EscapeUriString(roundCount.Value.ToString()) }");
            }
            if (customWords != null)
            {
                if (are_changes_specified)
                {
                    parameters_string_builder.Append("&");
                }
                else
                {
                    are_changes_specified = true;
                }
                parameters_string_builder.Append($"custom_words=");
                bool is_first = true;
                foreach (string custom_word in customWords)
                {
                    if (is_first)
                    {
                        is_first = false;
                    }
                    else
                    {
                        parameters_string_builder.Append(',');
                    }
                    parameters_string_builder.Append(Uri.EscapeUriString(roundCount.Value.ToString()));
                }
            }
            if (customWordsChance != null)
            {
                if (are_changes_specified)
                {
                    parameters_string_builder.Append("&");
                }
                else
                {
                    are_changes_specified = true;
                }
                parameters_string_builder.Append($"custom_words_chance={ Uri.EscapeUriString(customWordsChance.Value.ToString()) }");
            }
            if (isVotekickingEnabled != null)
            {
                if (are_changes_specified)
                {
                    parameters_string_builder.Append("&");
                }
                else
                {
                    are_changes_specified = true;
                }
                parameters_string_builder.Append($"enable_votekick={ Uri.EscapeUriString(isVotekickingEnabled.Value.ToString().ToLower()) }");
            }
            if (clientsPerIPLimit != null)
            {
                if (are_changes_specified)
                {
                    parameters_string_builder.Append("&");
                }
                else
                {
                    are_changes_specified = true;
                }
                parameters_string_builder.Append($"clients_per_ip_limit={ Uri.EscapeUriString(clientsPerIPLimit.Value.ToString()) }");
            }
            if (are_changes_specified)
            {
                await SendHTTPPATCHAsync(new Uri(http_host_uri, $"/v1/lobby?{ parameters_string_builder }"));
            }
            parameters_string_builder.Clear();
        }

        /// <summary>
        /// Dispose Scribble.rs client
        /// </summary>
        public void Dispose() => httpClient.Dispose();
    }
}
