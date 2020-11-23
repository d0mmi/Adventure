using System.Collections.Generic;

namespace Adventure.Server.GameLogic.Scenes
{
    public class Inventory
    {
        protected List<Item> Items;

        public Inventory()
        {
            Items = new List<Item>();
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }

        public Item Take(string name)
        {
            var item = Items.Find((i) => i.name == name);
            if (item != null && Items.Remove(item))
            {
                return item;
            }
            return null;
        }

        public int Count()
        {
            return Items.Count;
        }

        public List<Item> GetAll()
        {
            return Items;
        }
    }
}