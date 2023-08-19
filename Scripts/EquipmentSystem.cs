using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using static TextRPG.EquipmentSystem;

internal partial class TextRPG
{
    public class EquipmentSystem
    {

        public LinkedList<Equipment> ItemList { get; set; }
        
        [JsonConstructor]
        public EquipmentSystem()
        {
            ItemList = new LinkedList<Equipment>();
        }
        public void EquipItem(dynamic item)
        {
            if (IsEquipedItem(item))
            {
                // 장비 해제
                UnequipItem(item);
            }
            else
            {
                if (item is Weapon)
                {
                    // 무기 해제 -> 장착
                    var temp = item as Weapon;
                    if (temp != null)
                        ItemList.AddLast(new Equipment(temp));
                }
                else if (item is Armor)
                {
                    // 갑옷 해제 -> 장착
                    var temp = item as Armor;
                    if (temp != null)
                        ItemList.AddLast(new Equipment(temp));
                }
            }
        }

        public void UnequipItem(dynamic item)
        {
            foreach (var equipment in ItemList)
            {
                if (item == equipment.Data)
                    ItemList.Remove(equipment);
            }
        }

        public bool IsEquipedItem(dynamic item)
        {
            foreach (var equipment in ItemList)
            {
                if (item == equipment.Data)
                    return true;
            }
            return false;
        }

        public class Equipment
        {
            public dynamic Data { get; set; }
            [JsonConstructor]
            public Equipment(dynamic item)
            {
                Data = item;
            }
        }
    }
}
