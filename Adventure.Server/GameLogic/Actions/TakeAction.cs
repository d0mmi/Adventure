namespace Adventure.Server.GameLogic.Actions
{
    class TakeAction : Action
    {
        public TakeAction(MainGame game) : base("take", new TakeResult(game), "<item>")
        {

        }
    }

    class TakeResult : ActionResult
    {

        private MainGame game;

        public TakeResult(MainGame game)
        {
            this.game = game;
        }

        public override ActionResult Perform(string param)
        {
            var scene = game.GetCurrentScene();
            var inv = scene.Inventory;
            var item = inv.Take(param);
            if (item != null)
            {
                var player = game.player;
                player.Inventory.Add(item);
                Description = $"You added the Item '{param}' to your Inventory.";
                return this;
            }

            Description = $"You could not find the Item '{param}' on the ground!";
            return this;
        }
    }

}