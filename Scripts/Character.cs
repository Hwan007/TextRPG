using System.Text.Json.Serialization;

internal partial class TextRPG
{
    public class Character : IDisplay
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        private int baseAtk;
        public int Def { get; }
        private int baseDef;
        public int Hp { get; }
        private int baseHP;
        public int Gold { get; private set; }
        public Inventory Inven { get; }
        public bool IsDead { get => Hp <= 0 ? true : false; }
        public EquipmentSystem Equipments { get; }

        [JsonConstructor]
        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            baseAtk = Atk = atk;
            baseDef = Def = def;
            baseHP = Hp = hp;
            Gold = gold;
            Inven = new Inventory();
            Equipments = new EquipmentSystem();
        }

        public int Display()
        {
            // 플래이어 스탯 정보 표시
            Console.WriteLine();
            Console.WriteLine($"Lv.{Level}");
            Console.WriteLine($"{Name}({Job})");
            Console.WriteLine($"체력 : {Hp}");
            Console.WriteLine($"공격력 : {Atk}");
            Console.WriteLine($"방어력 : {Def}");
            Console.WriteLine($"Gold : {Gold}");
            Console.WriteLine();
            return 0;
        }

        public void TakeDamage()
        {

        }

        public void SellItem(int index)
        {
            var item = Inven.GetItem(index);
            if (item != null)
            {
                if (item.ValueRef.Data.IsEquip == true)
                    Equipments.UnequipItem(item);
                GetGold(item.ValueRef.Data.Gold);
                Inven.RemoveItem(index);
            }
        }

        public void BuyItem(dynamic item)
        {
            if (item != null)
            {
                Gold -= item.Gold;
                Inven.AddItem(item);
            }
            else
                Console.WriteLine("아이템 정보가 없습니다.");
        }

        public void GetGold(int earning)
        {
            Gold += earning;
        }
    }
}
