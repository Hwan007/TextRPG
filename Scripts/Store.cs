internal partial class TextRPG
{
    public class Store : IDisplay
    {
        public Inventory Inven { get; }

        public Store()
        {
            Inven = new Inventory();
        }
        public int Display()
        {
            return 1;
        }
    }
}
