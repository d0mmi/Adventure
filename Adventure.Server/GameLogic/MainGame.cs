using Adventure.Server.Sockets;
using Adventure.Server.GameLogic.Actions;
using Adventure.Server.GameLogic.Dialog;
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
        public event GameEventHandler<DialogResult> OnDialog;
        public event GameEventHandler<DialogElement> OnWrongDialogInput;

        public GameStatus Status { get; }
        public SocketConnection Client { get; private set; }

        public IReadOnlyDictionary<string, Scene> Scenes => _scenes;

        private Scene _currentScene;
        private DialogElement _currentDialog;
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

        public DialogElement GetCurrentDialog()
        {
            return _currentDialog;
        }

        public void SetCurrentDialog(DialogElement element)
        {
            _currentDialog = element;
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
            if (_currentDialog == null)
            {
                try
                {
                    var result = _currentScene.PerformAction(action);
                    if (_currentDialog != null)
                    {
                        var dialogResult = new DialogResult("", _currentDialog);
                        dialogResult.Description = _currentDialog.Text;
                        OnDialog?.Invoke(this, dialogResult);
                    }
                    else
                    {
                        OnAction?.Invoke(this, result);
                    }
                }
                catch (ActionNotValidException e)
                {
                    OnWrongInput?.Invoke(this, _currentScene);
                }
            }
            else
            {
                // TODO Error on end of conversation
                try
                {
                    var result = _currentDialog.Perform(action);
                    if (result == null || result.Description == null || result.Description.Length > 0 || result.Next == null)
                    {
                        _currentDialog = null;
                    }
                    else
                    {
                        _currentDialog = result.Next;
                    }
                    OnDialog?.Invoke(this, result);
                }
                catch (WrongDialogInputException e)
                {
                    OnWrongDialogInput?.Invoke(this, _currentDialog);
                }
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
            List<Npc> npcs = new List<Npc>();

            var options = new List<DialogResult>();
            options.Add(new YesDialogResult(new DialogElement("UwU, why are you so stupid...", new List<DialogResult>() { new OkDialogResult(null) })));
            options.Add(new NoDialogResult(new DialogElement("OwO, have a nice day. ^^", new List<DialogResult>() { new OkDialogResult(null) })));

            var dialog = new DialogElement("Are you stupid?", options);

            npcs.Add(new Npc("Jeremy-Pascal", "There is a big old guy with a very depressed look and a giant Chainsaw.", dialog));
            actions.Add(new SwitchSceneAction(new SwitchSceneResult(this, "house", "city"), "left", "right"));

            return new Scene("forest", description, actions, this, npcs);
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