namespace Adventure.Server.GameLogic.Actions
{
    class ConfrontAction : Action
    {
        public ConfrontAction(MainGame game) : base("confront", new ConfrontResult(game), "<person>")
        {

        }
    }

    class ConfrontResult : ActionResult
    {

        private MainGame game;

        public ConfrontResult(MainGame game)
        {
            this.game = game;
        }

        public override ActionResult Perform(string param)
        {
            var scene = game.GetCurrentScene();
            var npcs = scene.Npcs;
            if (npcs.Count > 0)
            {
                foreach (var npc in npcs)
                {
                    if (npc.Name == param)
                    {
                        game.SetCurrentDialog(npc.Dialog);
                        return this;
                    }
                }

                Description = "The person gets upset that you got his beautiful name wrong!";
                return this;
            }

            Description = $"There is nobody here ... \nYou should stop talking to yourself and speak to a doctor!";
            return this;
        }
    }

}