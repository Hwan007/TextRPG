using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using static TextRPG.EquipmentSystem;

internal partial class TextRPG
{
    public class EquipmentSystem
    {
        public LinkedList<EquipItemData> EquipItemList { get; }

        [JsonConstructor]
        public EquipmentSystem()
        {
            EquipItemList = new LinkedList<EquipItemData>();
        }
        public bool EquipItem(dynamic item)
        {
            bool IsEquiped = false;
            if (IsEquipedItem(item))
            {
                // 장비 해제
                UnequipItem(item);
                IsEquiped = false;
            }
            else
            {
                if (item is Weapon)
                {
                    // 무기 해제 -> 장착
                    var temp = item as Weapon;
                    if (temp != null)
                    {
                        EquipItemList.AddLast(new EquipItemData(item));
                        IsEquiped = true;
                    }
                        
                }
                else if (item is Armor)
                {
                    // 갑옷 해제 -> 장착
                    var temp = item as Armor;
                    if (temp != null)
                    {
                        EquipItemList.AddLast(new EquipItemData(item));
                        IsEquiped = true;
                    }
                }
            }
            return IsEquiped;
        }

        public void UnequipItem(dynamic item)
        {
            foreach (var equipment in EquipItemList)
            {
                if (item.GetType() == equipment.GetType && item == equipment.ItemRef)
                {
                    EquipItemList.Remove(equipment);
                    return;
                }
            }
        }

        public bool IsEquipedItem(dynamic item)
        {
            foreach (var equipment in EquipItemList)
            {
                if (item.GetType() == equipment.GetType && item == equipment.ItemRef)
                    return true;
            }
            return false;
        }

        public class EquipItemData
        {
            public dynamic ItemRef { get; }
            public Type GetType { get; }

            [JsonConstructor]
            public EquipItemData(dynamic item)
            {
                ItemRef = item;
                GetType = item.GetType();
            }
        }
    }
}
