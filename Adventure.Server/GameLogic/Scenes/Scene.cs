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

        public Scene(string id, string description, IEnumerable<Actions.Action> actions)
        {
            Id = id;
            Description = description;
            _actions.AddRange(actions);
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
                    return action.PerformIfVerbValid(actionString);
                }
                catch (System.Exception e)
                {

                    if (!(e is ActionNotValidException) && !(e is ParameterNotValidException))
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
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