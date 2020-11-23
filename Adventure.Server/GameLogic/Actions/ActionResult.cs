using System.Collections.Generic;

namespace Adventure.Server.GameLogic.Actions
{
    public abstract class ActionResult
    {
        public string Description { get; protected set; }


        public abstract ActionResult Perform(string param);
    }
}