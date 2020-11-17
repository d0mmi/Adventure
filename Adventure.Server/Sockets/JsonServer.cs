
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using Newtonsoft.Json;
using Adventure.Core.Commands;
using Adventure.Server.GameLogic;
using Adventure.Server.GameLogic.Scenes;

namespace Adventure.Server.Sockets
{
    public class JsonServer : AsyncSocketServer, ICommandSender
    {
        private Dictionary<Guid, MainGame> _runningGames;

        public JsonServer()
        {
            _runningGames = new Dictionary<Guid, MainGame>();
        }

        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public void SendCommand(Socket connection, ICommand command)
        {
            var msg = JsonConvert.SerializeObject(command, settings);
            SendMessage(connection, msg);
        }

        public void SendCommand(SocketConnection connection, ICommand command)
        {
            SendCommand(connection.GetClient(), command);
        }

        public void BroadcastCommand(ICommand command)
        {
            var i = 1;
            foreach (var con in connections)
            {
                Console.WriteLine($"Sending Reply to connection: [{con.GetID()}]..");
                SendCommand(con, command);
                i++;
            }
        }

        public void Send(ICommand command, Socket receiver)
        {
            SendCommand(receiver, command);
        }

        public override void OnMessageRecieved(SocketConnection connection, string msg)
        {
            var cmd = JsonConvert.DeserializeObject(msg, settings);
            if (cmd != null)
            {

                ((ICommand)cmd).ExecuteServer(this, connection.GetClient());
            }
            else
            {
                Console.WriteLine("Cmd was null!");
            }
        }

        public override void OnConnect(SocketConnection connection)
        {
            Console.WriteLine("New Connection found: " + connection.GetID());
        }

        public override void OnDisconnect(SocketConnection connection)
        {
            Console.WriteLine($"Client [{connection.GetID()}] disconnected: " + connection.GetClient().RemoteEndPoint.ToString());
            connections.Remove(connection);
        }

        public override void OnError(string msg)
        {
            Console.WriteLine("OnError: " + msg);
        }

        private MainGame GetOrCreateGame(Guid clientId)
        {
            if (!_runningGames.TryGetValue(clientId, out var game))
            {
                game = CreateGame();
                _runningGames.Add(clientId, game);

                return game;
            }

            Console.WriteLine($"Found existing Game for Client [{clientId}].");

            if (game.Status == GameStatus.Aborted || game.Status == GameStatus.Finished)
            {
                Console.WriteLine($"Existing Game for Client [{clientId}] was aborted or finished, restarting..");
                FinalizeGame(game);
                _runningGames.Remove(clientId);

                game = CreateGame();
                _runningGames.Add(clientId, game);
            }
            return game;
        }

        private MainGame CreateGame()
        {
            var game = new MainGame();
            // game.OnEnterScene += OnEnterScene;
            return game;
        }

        private void FinalizeGame(MainGame game)
        {
            // game.OnEnterScene -= OnEnterScene;
        }

        const string CallToActionText = "What do you want to do?";
        private void OnEnterScene(MainGame game, Scene scene)
        {
            Console.WriteLine($"[{game.Client.GetID()}] - OnEnterScene: {scene.Id}");
            Send(new PrintTextCommand($"{scene.Description}\n\n{CallToActionText}"), game.Client.GetClient());
            Send(new TextInputCommand(), game.Client.GetClient());
        }
    }
}