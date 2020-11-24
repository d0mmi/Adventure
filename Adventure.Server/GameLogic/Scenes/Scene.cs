using System;
using System.Collections.Generic;
using Adventure.Server.GameLogic.Actions;

namespace Adventure.Server.GameLogic.Scenes
{
    public class Scene
    {
        public string Id { get; }
        public string Description { get; }
        public Inventory Inventory { get; } = new Inventory();
        public IEnumerable<Actions.Action> Actions => _actions;

        private readonly List<Actions.Action> _actions = new List<Actions.Action>();
        public readonly List<Npc> Npcs = new List<Npc>();

        public Scene(string id, string description, List<Actions.Action> actions, MainGame game) : this(id, description, actions, game, new List<Npc>()) { }

        public Scene(string id, string description, List<Actions.Action> actions, MainGame game, List<Npc> npcs)
        {
            Id = id;
            Description = description;
            Npcs.AddRange(npcs);
            _actions.AddRange(actions);
            _actions.AddRange(new Actions.Action[]{
                new InvestigateAction(game),
                new TakeAction(game),
                new DropAction(game),
                new InventoryAction(game),
                new ConfrontAction(game),
                new HelpAction(_actions),
            });
        }

        public void Enter()
        {
        }



        public ActionResult PerformAction(string actionString)
        {
            foreach (var action in _actions)
            {
                try
                {
                    if (action != null)
                    {
                        var result = action.PerformIfVerbValid(actionString);
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("Action is null");
                    }
                }
                catch (System.Exception e)
                {

                    if (!(e is ActionNotValidException) && !(e is ParameterNotValidException))
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                        Console.WriteLine(e.Source);
                        Console.WriteLine(e.InnerException);
                    }
                }
            }
            throw new ActionNotValidException("No valid Action was found for this scene!");
        }

        public void Leave()
        {
        }
    }
}