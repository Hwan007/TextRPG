using System.Text;

internal partial class TextRPG
{
    public class StoreSystem
    {
        public InventorySystem Inven { get; }

        public StoreSystem()
        {
            Inven = new InventorySystem();
        }

        public void AddItems(Item[] items)
        {
            foreach (Item item in items)
            {
                Inven.AddItem(item);
            }
        }
        public StringBuilder[] Display()
        {
            return Inven.GetDisplayString(true);
        }

        public dynamic SellToPlayer(int index)
        {
            var item = Inven.GetItem(index);
            return item.ValueRef;
        }
    }
}
