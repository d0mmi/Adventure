using ConsoleTables;

namespace Adventure.Server.GameLogic.Actions
{
    class InvestigateAction : Action
    {
        public InvestigateAction(MainGame game) : base("investigate", new InvestigateResult(game))
        {

        }
    }

    class InvestigateResult : ActionResult
    {

        private MainGame game;

        public InvestigateResult(MainGame game)
        {
            this.game = game;
        }

        public override ActionResult Perform(string param)
        {
            var scene = game.GetCurrentScene();
            var inv = scene.Inventory;

            if (inv.Count() > 0)
            {
                var text = "You investigate this location, and find:\n";
                var table = new ConsoleTable("Ground");
                foreach (var item in inv.GetAll())
                {
                    table.AddRow(item.name);
                }
                text += table.ToString();
                Description = text;
                return this;
            }

            Description = "You investigate this location, but could not find anything.";
            return this;
        }
    }

}