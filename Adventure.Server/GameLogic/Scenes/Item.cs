using System;

namespace Adventure.Server.GameLogic.Scenes
{
    public class Item
    {
        protected Guid id;
        public string name { get; }


        public Item(string name)
        {
            id = Guid.NewGuid();
            this.name = name;
        }
    }
}