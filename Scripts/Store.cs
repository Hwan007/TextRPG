internal partial class TextRPG
{
    public class Store : IDisplay
    {
        public Inventory Inven { get; }

        public Store()
        {
            Inven = new Inventory();
        }

        public void AddItems(Item[] items)
        {
            foreach (Item item in items)
            {
                Inven.AddItem(item);
            }
        }
        public int Display()
        {
            return Inven.Display();
        }

        public dynamic SellToPlayer(int index)
        {
            var item = Inven.GetItem(index);
            return item.ValueRef;
        }
    }
}
