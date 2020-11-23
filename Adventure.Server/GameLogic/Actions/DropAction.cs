namespace Adventure.Server.GameLogic.Actions
{
    class DropAction : Action
    {
        public DropAction(MainGame game) : base("drop", new DropResult(game), "<item>")
        {

        }
    }

    class DropResult : ActionResult
    {

        private MainGame game;

        public DropResult(MainGame game)
        {
            this.game = game;
        }

        public override ActionResult Perform(string param)
        {
            var player = game.player;
            var inv = player.Inventory;
            var item = inv.Take(param);
            if (item != null)
            {
                var scene = game.GetCurrentScene();
                scene.Inventory.Add(item);
                Description = $"You dropped the Item '{param}' on the Ground.";
                return this;
            }

            Description = $"You could not find the Item '{param}' in your Inventory!";
            return this;
        }
    }

}