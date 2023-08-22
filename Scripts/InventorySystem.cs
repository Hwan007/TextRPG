using System.Text;

internal partial class TextRPG
{
    public class InventorySystem : DisplaySystem
    {
        private LinkedList<Item> mItems;
        public int Count { get => mItems.Count; }

        public InventorySystem()
        {
            mItems = new LinkedList<Item>();
        }

        /// <summary>
        /// 출력하고자 하는 StringBuilder 배열을 받는다.
        /// </summary>
        /// <param name="showGold">item.Gold를 보여주는지 여부</param>
        /// <returns></returns>
        public StringBuilder[] GetDisplayString(bool showGold)
        {
            AddStringToDisplayList("[아이템 목록]\n");
            // 인벤토리 내용물 표시
            int i = 0;
            List<StringBuilder> couts = new List<StringBuilder>();
            foreach (Item item in mItems)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"- ");
                sb.Append(item.GetDisplayString(showGold));
                sb.Append("\n");
                couts.Add(sb);
                ++i;
            }
            return couts.ToArray();
        }
        /// <summary>
        /// 보여주고자 하는 문자열을 DisplaySystem의 SBList에 넣는다.
        /// </summary>
        /// <param name="showGold">item.Gold를 보여주는지 여부</param>
        /// <returns></returns>
        public override int SetDisplayString(bool showGold = false)
        {
            AddStringToDisplayList("[아이템 목록]");
            // 인벤토리 내용물 표시
            int i = 0;
            foreach (Item item in mItems)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"- ");
                sb.Append(item.GetDisplayString(showGold));
                sb.Append("\n");
                AddStringToDisplayList(sb.ToString());
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
