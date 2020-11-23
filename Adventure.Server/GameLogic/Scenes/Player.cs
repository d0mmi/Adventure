namespace Adventure.Server.GameLogic.Scenes
{
    public class Player
    {
        public string Name { get; set; }
        public Inventory Inventory { get; }

        public Player()
        {
            Name = "";
            Inventory = new Inventory();
        }

    }
}