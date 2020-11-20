using System.Collections.Generic;

namespace Adventure.Server.GameLogic.Actions
{
    public abstract class ActionResult
    {
        public string Description { get; }


        public ActionResult Perform(string param)
        {
            return null;
        }
    }
}