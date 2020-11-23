namespace Adventure.Server.GameLogic.Actions
{
    class SwitchSceneAction : Action
    {
        public SwitchSceneAction(SwitchSceneResult result, params string[] parameters) : base("go", result, parameters)
        {

        }
    }

    class SwitchSceneResult : ActionResult
    {
        private string left;
        private string right;
        private string straight;
        private string back;

        private MainGame game;

        public SwitchSceneResult(MainGame game, string left = null, string right = null, string straight = null, string back = null)
        {
            this.game = game;
            this.left = left;
            this.right = right;
            this.straight = straight;
            this.back = back;
        }

        public override ActionResult Perform(string param)
        {
            Description = null;
            switch (param)
            {
                case "left":
                    if (left != null)
                        game.EnterScene(left);
                    return this;
                case "right":
                    if (right != null)
                        game.EnterScene(right);
                    return this;
                case "straight":
                    if (straight != null)
                        game.EnterScene(straight);
                    return this;
                case "back":
                    if (back != null)
                        game.EnterScene(back);
                    return this;
                default:
                    Description = "This way is not possible!";
                    return this;
            }
        }
    }

}