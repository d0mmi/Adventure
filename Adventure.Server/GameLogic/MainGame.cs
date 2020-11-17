using Adventure.Server.Sockets;
using Adventure.Server.GameLogic.Scenes;
using System.Collections.Generic;

namespace Adventure.Server.GameLogic
{

    public enum GameStatus
    {
        Running, Aborted, Finished
    }

    public class MainGame
    {
        public SocketConnection Client;
        public GameStatus Status;
        private Dictionary<string, Scene> _scenes;
        private Scene _currentScene;

        public void Start(SocketConnection connection)
        {

        }

        public void CreateScenes()
        {

        }

        public void EnterScene(string id)
        {
            OnEnterScene(this, null);
        }

        public void PerformAction(string action)
        {

        }


        public delegate void OnEnterScene(MainGame game, Scene scene);

    }
}