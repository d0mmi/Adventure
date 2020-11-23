namespace Adventure.Server.GameLogic.Actions
{
    class TextResult : ActionResult
    {

        public TextResult(string text)
        {
            Description = text;
        }

        public override ActionResult Perform(string param)
        {
            return this;
        }
    }
}