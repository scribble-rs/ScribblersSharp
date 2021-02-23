using NUnit.Framework;
using ScribblersSharp;
using ScribblersSharpTest.Data;
using System;
using System.Drawing;
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
            // ...
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
            string host_and_port = $"{ host }:{ port }";
            for (int index = 0; index < clients.Length; index++)
            {
                clients[index] = Clients.Create(host_and_port);
            }
            ILobby[] lobbies = new ILobby[clients.Length];
            Random random = new Random();
            string selected_word = string.Empty;
            bool keep_running = true;
            bool is_in_next_round = false;
            for (int index = 0; index < clients.Length; index++)
            {
                ILobby lobby;
                if (index == 0)
                {
                    lobby = clients[0].CreateLobbyAsync("TestClient_0", ELanguage.EnglishUS, true, Rules.maximalPlayers, Rules.maximalDrawingTime, Rules.maximalRounds, Array.Empty<string>(), Rules.minimalCustomWordsChance, false, Rules.maximalClientsPerIPLimit).GetAwaiter().GetResult();
                }
                else
                {
                    lobby = clients[index].EnterLobbyAsync(lobbies[0].LobbyID, "TestClient_" + index).GetAwaiter().GetResult();
                }
                Assert.IsNotNull(lobby);
                lobby.OnReadyGameMessageReceived += () =>
                {
                    Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" is ready.");
                    if ((lobby.Players.Count >= clients.Length) && (lobby.MyPlayer.ID == lobby.Owner.ID))
                    {
                        lobby.SendStartGameMessageAsync();
                    }
                };
                lobby.OnNextTurnGameMessageReceived += () =>
                {
                    Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" is in round \"{ lobby.Round }\".");
                    if (lobby.MyPlayer.ID == lobby.Owner.ID)
                    {
                        if (is_in_next_round)
                        {
                            keep_running = false;
                        }
                        else
                        {
                            is_in_next_round = true;
                        }
                    }
                };
                lobby.OnUpdatePlayersGameMessageReceived += (players) =>
                {
                    Console.WriteLine($"Players for client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" has been updated.");
                    if ((players.Count >= clients.Length) && (lobby.MyPlayer.ID == lobby.Owner.ID))
                    {
                        lobby.SendStartGameMessageAsync();
                    }
                };
                lobby.OnUpdateWordhintGameMessageReceived += (wordHints) => Console.WriteLine($"Word hints for client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" has been updated.");
                lobby.OnMessageGameMessageReceived += (author, content) => Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" has received a player message from \"{ author.Name }\" with ID \"{ author.ID }\": \"{ content }\"");
                lobby.OnNonGuessingPlayerMessageGameMessageReceived += (author, content) => Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" has received a non-guessing player message from \"{ author.Name }\" with ID \"{ author.ID }\": \"{ content }\"");
                lobby.OnSystemMessageGameMessageReceived += (content) => Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" has received a system message: \"{ content }\"");
                lobby.OnLineGameMessageReceived += (fromX, fromY, toX, toY, color, lineWidth) =>
                {
                    Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" has received a line draw command: From: ({ fromX }, { fromY }); To: ({ toX }, { toY }); Color: ({ color.R }, { color.G }, { color.B }); Line width: { lineWidth }");
                    if (!lobby.IsPlayerAllowedToDraw)
                    {
                        lobby.SendMessageGameMessageAsync(selected_word);
                    }
                };
                lobby.OnFillGameMessageReceived += (positionX, positionY, color) =>
                {
                    Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" has received a fill draw command: Position: ({ positionX }, { positionY }); Color: ({ color.R }, { color.G }, { color.B })");
                    if (!lobby.IsPlayerAllowedToDraw)
                    {
                        lobby.SendMessageGameMessageAsync(selected_word);
                    }
                };
                lobby.OnClearDrawingBoardGameMessageReceived += () => Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" has received a clear drawing board command.");
                lobby.OnYourTurnGameMessageReceived += (words) =>
                {
                    Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" is now choosing a word.");
                    Assert.NotZero(words.Count);
                    int selected_word_index = random.Next(words.Count);
                    selected_word = words[selected_word_index];
                    lobby.SendChooseWordGameMessageAsync((uint)selected_word_index).GetAwaiter().GetResult();
                    if (random.Next(2) == 0)
                    {
                        lobby.SendFillGameMessageAsync(lobby.DrawingBoardBaseWidth * (float)random.NextDouble(), lobby.DrawingBoardBaseHeight * (float)random.NextDouble(), Color.FromArgb(random.Next(0x100), random.Next(0x100), random.Next(0x100)));
                    }
                    else
                    {
                        lobby.SendLineGameMessageAsync(lobby.DrawingBoardBaseWidth * (float)random.NextDouble(), lobby.DrawingBoardBaseHeight * (float)random.NextDouble(), lobby.DrawingBoardBaseWidth * (float)random.NextDouble(), lobby.DrawingBoardBaseHeight * (float)random.NextDouble(), Color.FromArgb(random.Next(0x100), random.Next(0x100), random.Next(0x100)), 40.0f * (float)random.NextDouble());
                    }
                };
                lobby.OnCorrectGuessGameMessageReceived += (player) => Console.WriteLine($"For client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" player \"{ player.Name }\" with ID \"{ player.ID }\" has guessed right.");
                lobby.OnDrawingGameMessageReceived += (currentDrawing) => Console.WriteLine($"Client \"{ lobby.MyPlayer.Name }\" with ID \"{ lobby.MyPlayer.ID }\" got the current drawing.");
                lobby.OnUnknownGameMessageReceived += (gameMessage, json) => Assert.Fail($"Unknown game message \"{ gameMessage.MessageType }\" received.{ Environment.NewLine }{ Environment.NewLine }JSON:{ Environment.NewLine }{ json }");
                lobbies[index] = lobby;
            }
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
