using NUnit.Framework;
using ScribblersSharp;
using ScribblersSharpTest.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text.Json;

/// <summary>
/// Scribble.rs ♯ test namespace
/// </summary>
namespace ScribblersSharpTest
{
    /// <summary>
    /// Unit tests class
    /// </summary>
    public class UnitTests
    {
        /// <summary>
        /// Default host
        /// </summary>
        private static readonly string defaultHost = "localhost";

        /// <summary>
        /// Default port
        /// </summary>
        private static readonly ushort defaultPort = 8080;

        /// <summary>
        /// Configuration path
        /// </summary>
        private static readonly string configurationPath = "./config.json";

        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Test Scribble.rs clients
        /// </summary>
        [Test]
        public void TestScribblersClients()
        {
            string host = defaultHost;
            ushort port = defaultPort;
            if (File.Exists(configurationPath))
            {
                try
                {
                    using FileStream file_stream = File.OpenRead(configurationPath);
                    using StreamReader reader = new StreamReader(file_stream);
                    ConfigurationData configuration_data = JsonSerializer.Deserialize<ConfigurationData>(reader.ReadToEnd());
                    if (configuration_data != null)
                    {
                        if (configuration_data.Host != null)
                        {
                            host = configuration_data.Host;
                        }
                        port = configuration_data.Port;
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
            IScribblersClient[] clients = new IScribblersClient[Rules.maximalPlayers];
            string host_and_port = host + ":" + port;
            for (int index = 0; index < clients.Length; index++)
            {
                clients[index] = Clients.Create(host_and_port);
            }
            ILobby[] lobbies = new ILobby[clients.Length];
            for (int index = 0; index < clients.Length; index++)
            {
                ILobby lobby;
                if (index == 0)
                {
                    lobby = clients[0].CreateLobbyAsync("TestClient_0", ELanguage.English, true, Rules.maximalPlayers, Rules.maximalDrawingTime, Rules.maximalRounds, Array.Empty<string>(), Rules.minimalCustomWordsChance, false, Rules.maximalClientsPerIPLimit).GetAwaiter().GetResult();
                }
                else
                {
                    lobby = clients[index].EnterLobbyAsync(lobbies[0].LobbyID, "TestClient_" + index).GetAwaiter().GetResult();
                }
                Assert.IsNotNull(lobby);
                lobby.OnReadyGameMessageReceived += (playerID, isDrawing, ownerID, round, maximalRounds, roundEndTime, wordHints, players, currentDrawing, gameState) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnNextTurnGameMessageReceived += (players, round, roundEndTime) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnUpdatePlayersGameMessageReceived += (players) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnUpdateWordhintGameMessageReceived += (wordHints) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnMessageGameMessageReceived += (author, content) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnNonGuessingPlayerMessageGameMessageReceived += (author, content) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnSystemMessageGameMessageReceived += (content) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnLineGameMessageReceived += (fromX, fromY, toX, toY, color, lineWidth) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnFillGameMessageReceived += (positionX, positionY, color) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnClearDrawingBoardGameMessageReceived += () =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnYourTurnGameMessageReceived += (words) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnCorrectGuessGameMessageReceived += (words) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobbies[index] = lobby;
            }
            lobbies[0].SendStartGameAsync().GetAwaiter().GetResult();
            bool keep_running = true;
            while (keep_running)
            {
                foreach (ILobby lobby in lobbies)
                {
                    if (lobby.WebSocketState != WebSocketState.Open)
                    {
                        keep_running = false;
                        break;
                    }
                    lobby.ProcessEvents();
                }
            }
            foreach (ILobby lobby in lobbies)
            {
                lobby.Dispose();
            }
        }
    }
}
