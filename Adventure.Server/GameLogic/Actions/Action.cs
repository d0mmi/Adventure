namespace Adventure.Server.GameLogic.Actions
{
    public abstract class Action
    {
        protected string Verb;


        public ActionResult Perform(string param)
        {
            return null;
        }
    }
}