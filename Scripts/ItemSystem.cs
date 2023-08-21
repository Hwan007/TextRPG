using static TextRPG;
using System.Text.Json.Serialization;
using System.Text;

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
        public int Gold;
    }

    public class Item
    {
        public ItemData Data { get; protected set; }
        public string Name { get => Data.Name; }
        public string Description { get => Data.Description; }
        public bool IsEquip
        {
            get => Data.IsEquip;
            private set
            {
                var tempData = Data;
                tempData.IsEquip = value;
                Data = tempData;
            }
        }
        public int Gold { get => Data.Gold; }

        [JsonConstructor]
        public Item(string name, string description, int gold)
        {
            Data = new ItemData()
            {
                Name = name,
                IsEquip = false,
                Description = description,
                Gold = gold
            };
        }
        public Item(ItemData data)
        {
            Data = data;
        }
        public Item(Item item)
        {
            Data = item.Data;
        }

        public virtual int Display(bool showGold)
        {
            // 장착 여부 표시
            if (IsEquip)
                Console.Write("[E] ");
            // 이름을 표시
            StringBuilder cout = new StringBuilder();
            cout.Append(Name);
            while (cout.Length < 14 - Name.Length)
            {
                cout.Append(" ");
            }
            Console.Write(cout);
            return 1;
        }

        public void EquipByCharacter(Character character)
        {
            IsEquip = character.Equipments.EquipItem(this);
        }
        public void UnquipByCharacter(Character character)
        {
            if (IsEquip == false)
                return;
            character.Equipments.UnequipItem(this);
            IsEquip = false;
        }
        public void SetPrice(int gold)
        {
            var tempData = Data;
            tempData.Gold = gold;
            Data = tempData;
        }
    }

    public class Weapon : Item
    {
        public int ATK { get => Data.Point; }
        [JsonConstructor]
        public Weapon(string name, int atk, string description, int gold) : base(name, description, gold)
        {
            var tempData = Data;
            tempData.type = EquipType.Weapon;
            tempData.Point = atk;
            Data = tempData;
        }
        public Weapon(ItemData data) : base(data) { }
        public Weapon(Item data) : base(data) { }

        public override int Display(bool showGold)
        {
            base.Display(showGold);
            // 공격력 표시
            StringBuilder cout = new StringBuilder();
            cout.Append("| 공격력 +");
            cout.Append(ATK);
            while (cout.Length < 10)
            {
                cout.Append(" ");
            }
            Console.Write(cout);
            // 설명 표시
            cout.Clear();
            cout.Append(" | ");
            cout.Append(Description);
            int spaceCount = Description.Count(c => c.Equals(' '));
            while (cout.Length < 50 - Description.Length + spaceCount)
            {
                cout.Append(" ");
            }
            Console.Write(cout);
            // 금액 표시
            if (showGold)
            {
                cout.Clear();
                cout.Append(" | ");
                cout.Append(Gold);
                cout.Append(" G");
                Console.Write(cout);
            }
            return 1;
        }
    }

    public class Armor : Item
    {
        public int DEF { get => Data.Point; }
        [JsonConstructor]
        public Armor(string name, int def, string description, int gold) : base(name, description, gold)
        {
            var tempData = Data;
            tempData.type = EquipType.Armor;
            tempData.Point = def;
            Data = tempData;
        }
        public Armor(ItemData data) : base(data) { }
        public Armor(Item data) : base(data) { }
        public override int Display(bool showGold)
        {
            base.Display(showGold);
            // 방어력 표시
            StringBuilder cout = new StringBuilder();
            cout.Append("| 방어력 +");
            cout.Append(DEF);
            while (cout.Length < 10)
            {
                cout.Append(" ");
            }
            Console.Write(cout);
            // 설명 표시
            cout.Clear();
            cout.Append(" | ");
            cout.Append(Description);
            int spaceCount = Description.Count(c => c.Equals(' '));
            while (cout.Length < 50 - Description.Length + spaceCount)
            {
                cout.Append(" ");
            }
            Console.Write(cout);
            // 금액 표시
            if (showGold)
            {
                cout.Clear();
                cout.Append(" | ");
                cout.Append(Gold);
                cout.Append(" G");
                Console.Write(cout);
            }
            return 1;
        }
    }
}