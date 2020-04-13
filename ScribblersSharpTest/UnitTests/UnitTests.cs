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
                    using (FileStream file_stream = File.OpenRead(configurationPath))
                    {
                        using (StreamReader reader = new StreamReader(file_stream))
                        {
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
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
            ScribblersClient[] clients = new ScribblersClient[Rules.maximalPlayers];
            for (int index = 0; index < clients.Length; index++)
            {
                clients[index] = new ScribblersClient();
            }
            string host_and_port = host + ":" + port;
            ILobby[] lobbies = new ILobby[clients.Length];
            for (int index = 0; index < clients.Length; index++)
            {
                ILobby lobby;
                if (index == 0)
                {
                    lobby = clients[0].CreateLobbyAsync(host_and_port, "TestClient_0", ELanguage.English, Rules.maximalPlayers, Rules.maximalDrawingTime, Rules.maximalRounds, Array.Empty<string>(), Rules.minimalCustomWordsChance, false, Rules.maximalClientsPerIPLimit).GetAwaiter().GetResult();
                }
                else
                {
                    lobby = clients[index].EnterLobbyAsync(host_and_port, lobbies[0].LobbyID, "TestClient_" + index).GetAwaiter().GetResult();
                }
                Assert.IsNotNull(lobby);
                lobby.OnClearDrawingBoardGameMessageReceived += () =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnFillDrawnGameMessageReceived += (positionX, positionY, color) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnGuessingChatMessageGameMessageReceived += (author, content) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnLineDrawnGameMessageReceived += (fromX, fromY, toX, toY, color, lineWidth) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnNextTurnGameMessageReceived += (players, round, roundEndTime) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnNonGuessingChatMessageGameMessageReceived += (author, content) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnReadyGameMessageReceived += (playerID, isDrawing, ownerID, round, maximalRounds, roundEndTime, wordHints, players, currentDrawing) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnSystemMessageGameMessageReceived += (content) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnUpdatePlayersGameMessageReceived += (players) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnUpdateWordHintsGameMessageReceived += (wordHints) =>
                {
                    // TODO
                    Debug.WriteLine(Environment.StackTrace);
                };
                lobby.OnYourTurnGameMessageReceived += (words) =>
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
