internal partial class TextRPG
{
    public class Store : IDisplay
    {
        public Inventory Inven { get; }

        public Store(Item[] items)
        {
            Inven = new Inventory();
            foreach (Item item in items)
            {
                Inven.AddItem(item);
            }
        }
        public int Display()
        {
            return Inven.Display();
        }

        public void SellItem(int index)
        {
            
        }

        public void BuyItem(int index)
        {
            
        }
    }
}
