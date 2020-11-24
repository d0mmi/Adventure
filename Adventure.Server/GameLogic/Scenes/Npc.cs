using Adventure.Server.GameLogic.Dialog;

namespace Adventure.Server.GameLogic.Scenes
{
    public class Npc
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Inventory Inventory { get; }

        public DialogElement Dialog;

        public Npc(string name, string description, DialogElement dialogElement)
        {
            Name = name;
            Description = description;
            Dialog = dialogElement;
            Inventory = new Inventory();
        }

    }
}