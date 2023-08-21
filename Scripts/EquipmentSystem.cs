using System.Drawing;
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
                    // 무기 해제
                    foreach (var equipment in EquipItemList)
                    {
                        if (equipment.ItemRef is Weapon)
                        {
                            UnequipItem(equipment.ItemRef);
                            break;
                        }
                    }
                    // 무기 장착
                    var itemWeapon = item as Weapon;
                    if (itemWeapon != null)
                    {
                        EquipItemList.AddLast(new EquipItemData(itemWeapon));
                        IsEquiped = true;
                    }
                }
                else if (item is Armor)
                {
                    // 갑옷 해제
                    foreach (var equipment in EquipItemList)
                    {
                        if (equipment.ItemRef is Armor)
                        {
                            UnequipItem(equipment.ItemRef);
                            break;
                        }
                    }
                    // 갑옷 장착
                    var itemArmor = item as Armor;
                    if (itemArmor != null)
                    {
                        EquipItemList.AddLast(new EquipItemData(itemArmor));
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
                if (item.GetType() == equipment.ItemRef.GetType() && item == equipment.ItemRef)
                {
                    EquipItemList.Remove(equipment);
                    equipment.ItemRef.Unequip();
                    return;
                }
            }
        }

        public bool IsEquipedItem(dynamic item)
        {
            foreach (var equipment in EquipItemList)
            {
                if (item.GetType() == equipment.ItemRef.GetType() && item == equipment.ItemRef)
                    return true;
            }
            return false;
        }

        public class EquipItemData
        {
            public dynamic ItemRef { get; }

            [JsonConstructor]
            public EquipItemData(dynamic item)
            {
                ItemRef = item;
            }
        }
    }
}
