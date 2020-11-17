using Adventure.Server.Sockets;
using Adventure.Server.GameLogic.Scenes;
using System.Collections.Generic;

namespace Adventure.Server.GameLogic
{
    public class MainGame
    {
        SocketConnection Client;
        private Dictionary<string, Scene> _scenes;
        private Scene _currentScene;

        public void Start(SocketConnection connection)
        {

        }

        public void CreateScenes()
        {

        }

        public void EnderScene(string id)
        {

        }

        public void PerformAction(string action)
        {

        }
    }
}