using System.Collections.Generic;
using Adventure.Server.GameLogic.Actions;

namespace Adventure.Server.GameLogic.Scenes
{
    public class Scene
    {
        public string Description;
        protected Inventory sceneInventory;
        protected IEnumerable<Action> Actions;


        public void Enter()
        {

        }

        public void Leave()
        {

        }
    }
}