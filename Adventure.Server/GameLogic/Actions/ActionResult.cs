using System.Collections.Generic;

namespace Adventure.Server.GameLogic.Actions
{
    public abstract class ActionResult
    {
        protected string Description;


        public ActionResult Perform(string param)
        {
            return null;
        }
    }
}