
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using Newtonsoft.Json;
using Adventure.Core.Commands;
using Adventure.Server.GameLogic;
using Adventure.Server.GameLogic.Actions;
using Adventure.Server.GameLogic.Scenes;

namespace Adventure.Server.Sockets
{
    public class JsonServer : AsyncSocketServer, ICommandSender
    {
        private Dictionary<Guid, MainGame> _runningGames = new Dictionary<Guid, MainGame>();

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
            var command = (ICommand)JsonConvert.DeserializeObject(msg, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            var game = GetOrCreateGame(connection.GetID());
            if (command is ClientConnectedCommand)
            {
                Console.WriteLine("command is ClientConnectedCommand");
                game.Start(connection);
            }
            else if (command is TextInputCommand)
            {
                game.PerformAction((command as TextInputCommand).Response);
            }
            else
            {
                Console.WriteLine("command is unknown");
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
            game.OnEnterScene += OnEnterScene;
            game.OnWrongInput += OnWrongInput;
            game.OnAction += OnAction;
            return game;
        }

        private void FinalizeGame(MainGame game)
        {
            game.OnEnterScene += OnEnterScene;
            game.OnWrongInput += OnWrongInput;
            game.OnAction += OnAction;
        }

        const string clearText = "\n\n\n\n\n\n\n\n\n\n";
        const string CallToActionText = "What do you want to do?";
        const string InvalidInputText = "Your Input was not valid!";
        private void OnEnterScene(MainGame game, Scene scene)
        {
            Console.WriteLine($"[{game.Client.GetID()}] - OnEnterScene: {scene.Id}");
            Send(new PrintTextCommand($"{clearText}{scene.Description}\n\n{CallToActionText}"), game.Client.GetClient());
            Send(new TextInputCommand(), game.Client.GetClient());
        }

        private void OnWrongInput(MainGame game, Scene scene)
        {
            Console.WriteLine($"[{game.Client.GetID()}] - OnWrongInput: {scene.Id}");
            Send(new PrintTextCommand($"{clearText}{scene.Description}\n\n{InvalidInputText}\n\n{CallToActionText}"), game.Client.GetClient());
            Send(new TextInputCommand(), game.Client.GetClient());
        }

        private void OnAction(MainGame game, ActionResult result)
        {
            Console.WriteLine($"[{game.Client.GetID()}] - OnAction");
            Send(new PrintTextCommand($"{result.Description}\n\n{CallToActionText}"), game.Client.GetClient());
            Send(new TextInputCommand(), game.Client.GetClient());
        }
    }
}