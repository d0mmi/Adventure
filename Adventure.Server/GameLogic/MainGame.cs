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
        private Player _player;

        private readonly Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();
        public MainGame()
        {
            CreateNewGame();
            _player = new Player();
        }
        public void Start(SocketConnection connection)
        {
            Client = connection;
            EnterScene("forest");
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
                OnAction?.Invoke(this, _currentScene.PerformAction(action));
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
        }

        private void AddScene(Scene scene)
        {
            _scenes.Add(scene.Id, scene);
        }

        private Scene CreateForestScene()
        {
            var description = @"Du stehst in einem dichten Wald. Die Sonne verbirgt sich über großem Geäst. 
In der Nähe zwitschern einige Vögel eine dir seltsam bekannte Melodie.
Der Weg zu deiner Rechten führt weiter an ein unscheinbares Haus, aus dessen kleinem Kamin dichter Rauch quillt.
Zu deiner Linken befindet sich ein tiefer Abgrund, bei dessen Anblick dir ein kalter Schauer über den Rücken läuft.";
            var actions = new[]
            {
                new Action("gehe", null, "links", "rechts"),
            };

            return new Scene("forest", description, actions);
        }
    }
}