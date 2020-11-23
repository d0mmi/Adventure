using Adventure.Server.Sockets;
using Adventure.Server.GameLogic.Actions;
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

        public delegate void GameEventHandler<T>(MainGame game, T arg);

        public event GameEventHandler<Scene> OnEnterScene;
        public event GameEventHandler<Scene> OnWrongInput;
        public event GameEventHandler<ActionResult> OnAction;

        public GameStatus Status { get; }
        public SocketConnection Client { get; private set; }

        public IReadOnlyDictionary<string, Scene> Scenes => _scenes;

        private Scene _currentScene;
        public Player player { get; }

        private readonly Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();
        public MainGame()
        {
            CreateNewGame();
            player = new Player();
        }
        public void Start(SocketConnection connection)
        {
            Client = connection;
            EnterScene("forest");
        }

        public Scene GetCurrentScene()
        {
            return _currentScene;
        }

        public void EnterScene(string id)
        {
            if (_scenes.TryGetValue(id, out var scene))
            {
                _currentScene = scene;
                OnEnterScene?.Invoke(this, _currentScene);
                return;
            }

            System.Console.WriteLine($"[ERROR] Cannot find scene '{id}'.");
        }

        public void PerformAction(string action)
        {
            try
            {
                var result = _currentScene.PerformAction(action);
                OnAction?.Invoke(this, result);
            }
            catch (ActionNotValidException e)
            {
                OnWrongInput?.Invoke(this, _currentScene);
            }
        }

        private void CreateNewGame()
        {
            CreateScenes();
        }

        private void CreateScenes()
        {
            AddScene(CreateForestScene());
            AddScene(CreateHouseScene());
            AddScene(CreateCityScene());
        }

        private void AddScene(Scene scene)
        {
            _scenes.Add(scene.Id, scene);
        }

        private Scene CreateForestScene()
        {
            var description = @"You are in a dark forest.";

            List<Action> actions = new List<Action>();
            actions.Add(new SwitchSceneAction(new SwitchSceneResult(this, "house", "city"), "left", "right"));

            return new Scene("forest", description, actions, this);
        }

        private Scene CreateHouseScene()
        {
            var description = @"You are approching a big scary house.";

            List<Action> actions = new List<Action>();
            actions.Add(new SwitchSceneAction(new SwitchSceneResult(this, null, null, null, "forest"), "back"));
            var house = new Scene("house", description, actions, this);
            house.Inventory.Add(new Item("Useless Stick"));
            return house;
        }

        private Scene CreateCityScene()
        {
            var description = @"You are entering a beautiful City.";

            List<Action> actions = new List<Action>();
            actions.Add(new SwitchSceneAction(new SwitchSceneResult(this, null, null, null, "forest"), "back"));

            return new Scene("city", description, actions, this);
        }
    }
}