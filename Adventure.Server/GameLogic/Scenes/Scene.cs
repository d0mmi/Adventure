using System.Collections.Generic;
using Adventure.Server.GameLogic.Actions;

namespace Adventure.Server.GameLogic.Scenes
{
    public class Scene
    {
        public string Id { get; }
        public string Description { get; }
        public Inventory Inventory { get; } = new Inventory();
        public IEnumerable<Action> Actions => _actions;

        private readonly List<Action> _actions = new List<Action>();

        public Scene(string id, string description, IEnumerable<Action> actions)
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
                        throw e;
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