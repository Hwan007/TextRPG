using System.Text;

internal partial class TextRPG
{
    public class Inventory : IDisplay
    {
        private LinkedList<Item> mItems;

        public Inventory()
        {
            mItems = new LinkedList<Item>();
        }
        public int Display()
        {
            Console.WriteLine("[아이템 목록]");
            // 인벤토리 내용물 표시
            int i = 0;
            StringBuilder cout = new StringBuilder();
            foreach (Item item in mItems)
            {
                cout.Clear();
                cout.Append($"- ");
                if (i < 10)
                    cout.Append(" ");
                cout.Append($"{i} ");
                Console.Write(cout);
                item.Display();
                Console.Write("\n");
                ++i;
            }
            return i;
        }
        public void AddItem(Item addItem)
        {
            if (addItem.Data.type == EquipType.Weapon)
            {
                var item = new Weapon(addItem.Data);
                mItems.AddLast(item);
            }
            else if (addItem.Data.type == EquipType.Armor)
            {
                var item = new Armor(addItem.Data);
                mItems.AddLast(item);
            }
            else
                throw new Exception($"{addItem.Data.type} => What is this?");
        }
        public LinkedListNode<Item>? GetItem(int index)
        {
            var item = mItems.First;
            if (item is LinkedListNode<Item>)
            {
                for (int i = 0; i < index; ++i)
                {
                    if (item.Next != null)
                        item = item.Next;
                    else
                        throw new Exception($"Item is null at {i}, but try to Get");
                }
                return item;
            }
            return null;
        }

        public void RemoveItem(int index)
        {
            var item = mItems.First;
            if (item is LinkedListNode<Item>)
            {
                for (int i = 0; i < index; ++i)
                {
                    if (item.Next != null)
                        item = item.Next;
                    else
                        throw new Exception($"Item is null at {i}, but try to Remove");
                }
                mItems.Remove(item);
            }
        }
    }
}
