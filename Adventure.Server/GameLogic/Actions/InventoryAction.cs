using ConsoleTables;

namespace Adventure.Server.GameLogic.Actions
{
    class InventoryAction : Action
    {
        public InventoryAction(MainGame game) : base("inventory", new InventoryResult(game))
        {

        }
    }

    class InventoryResult : ActionResult
    {

        private MainGame game;

        public InventoryResult(MainGame game)
        {
            this.game = game;
        }

        public override ActionResult Perform(string param)
        {
            var player = game.player;
            var inv = player.Inventory;

            if (inv.Count() > 0)
            {
                var text = "You look into your inventory:\n";
                var table = new ConsoleTable("Inventory");
                foreach (var item in inv.GetAll())
                {
                    table.AddRow(item.name);
                }
                text += table.ToString();
                Description = text;
                return this;
            }

            Description = "You look into your inventory, but it is empty.";
            return this;
        }
    }

}