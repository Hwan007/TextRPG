using static TextRPG;
using System.Text.Json.Serialization;

internal partial class TextRPG
{
    public enum EquipType
    {
        Weapon,
        Armor
    }

    public struct ItemData
    {
        public EquipType type;
        public string Name;
        public string Description;
        public bool IsEquip;
        public int Point;
    }

    public class Item : IDisplay
    {
        protected ItemData mData;
        public ItemData Data { get { return mData; } }
        public string Name { get => mData.Name; }
        public string Description { get => mData.Description; }
        public bool IsEquip { get => mData.IsEquip; set => mData.IsEquip = value; }

        [JsonConstructor]
        public Item(string name, string description)
        {
            mData = new ItemData()
            {
                Name = name,
                IsEquip = false,
                Description = description
            };
        }
        public Item(ItemData data)
        {
            mData = data;
        }
        public Item(Item item)
        {
            mData = item.Data;
        }

        public virtual int Display()
        {
            // 장착 여부 표시
            if (IsEquip)
                Console.Write("[E]");
            // 이름을 표시
            Console.Write(Name);
            return 1;
        }
    }

    public class Weapon : Item
    {
        public int ATK { get => mData.Point; }
        [JsonConstructor]
        public Weapon(string name, int atk, string description) : base(name, description)
        {
            mData.type = EquipType.Weapon;
            mData.Point = atk;
        }
        public Weapon(ItemData data) : base(data) { }
        public Weapon(Item data) : base(data) { }

        public override int Display()
        {
            base.Display();
            // 공격력 표시
            Console.Write("공격력 +" + ATK);
            // 설명 표시
            Console.Write(Description);
            return 1;
        }
    }

    public class Armor : Item
    {
        public int DEF { get => mData.Point; }
        [JsonConstructor]
        public Armor(string name, int def, string description) : base(name, description)
        {
            mData.type = EquipType.Armor;
            mData.Point = def;
        }
        public Armor(ItemData data) : base(data) { }
        public Armor(Item data) : base(data) { }
        public override int Display()
        {
            base.Display();
            // 방어력 표시
            Console.Write("방어력 +" + DEF);
            // 설명 표시
            Console.Write(Description);
            return 1;
        }
    }
}