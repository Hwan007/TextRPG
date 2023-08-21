using System.Text.Json.Serialization;

internal partial class TextRPG
{
    public class Character : IDisplay
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; private set; }
        public float Atk
        {
            get
            {
                float result = baseAtk;
                foreach (var item in Equipments.EquipItemList)
                {
                    if (item.ItemRef is Weapon)
                    {
                        result += item.ItemRef.Data.Point;
                    }
                }
                return result;
            }
        }
        private float baseAtk;
        public float Def
        {
            get
            {
                float result = baseDef;
                foreach (var item in Equipments.EquipItemList)
                {
                    if (item.ItemRef is Armor)
                    {
                        result += item.ItemRef.Data.Point;
                    }
                }
                return result;
            }
        }
        private float baseDef;
        public int Hp { get; private set; }
        private int baseHP;
        public int Gold { get; private set; }
        public Inventory Inven { get; }
        public bool IsDead { get => Hp <= 0 ? true : false; }
        public EquipmentSystem Equipments { get; }
        public int Exp { get; private set; }

        [JsonConstructor]
        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            baseAtk = atk;
            baseDef = def;
            baseHP = Hp= hp;
            Gold = gold;
            Inven = new Inventory();
            Equipments = new EquipmentSystem();
        }

        public int Display()
        {
            // 플래이어 스탯 정보 표시
            //Console.WriteLine(" ===================");
            //Console.WriteLine($" Lv.{Level}");
            //Console.WriteLine($" {Name} ({Job})");
            //Console.WriteLine($" 체력   : {Hp}");
            //int tmpAtk = Atk;
            //Console.WriteLine(" 공격력 : {0} {1}", tmpAtk, tmpAtk > baseAtk ? $" (+{tmpAtk - baseAtk})" : "");
            //int tmpDef = Def;
            //Console.WriteLine(" 방어력 : {0} {1}", tmpDef, tmpDef > baseDef ? $" (+{tmpDef - baseDef})" : "");
            //Console.WriteLine($" Gold   : {Gold}");
            //Console.WriteLine(" ===================");
            WriteWithCustomColor(" ===================\n");
            WriteWithCustomColor($" Lv.{Level}\n");
            WriteWithCustomColor($" {Name, -4} ({Job})\n");
            WriteWithCustomColor($" {"체력",-6}   : {Hp}/{baseHP}\n");
            float tmpAtk = Atk;
            WriteWithCustomColor($" {"공격력",-7} : {tmpAtk} {(tmpAtk > baseAtk ? $" (+{tmpAtk - baseAtk})" : "")}\n");
            float tmpDef = Def;
            WriteWithCustomColor($" {"방어력",-7} : {tmpDef} {(tmpDef > baseDef ? $" (+{tmpDef - baseDef})" : "")}\n");
            WriteWithCustomColor($" {"골드",-6}   : ");
            WriteWithCustomColor($"{Gold:N0} G\n", 178);
            WriteWithCustomColor(" ===================\n");
            return 0;
        }

        public void TakeDamage(int damage)
        {
            if (Hp - damage <= 0)
            {
                Hp = 0;
            }
            else
                Hp -= damage;
        }

        public void TakeHeal(int heal)
        {
            Hp = Hp + heal >= baseHP ? baseHP : Hp + heal;
        }

        public void SellItem(int index)
        {
            var item = Inven.GetItem(index);
            if (item != null)
            {
                if (item.ValueRef.IsEquip == true)
                    Equipments.UnequipItem(item.ValueRef);
                GetGold(item.ValueRef.Gold);
                Inven.RemoveItem(index);
            }
        }

        public void BuyItem(dynamic item)
        {
            if (item != null)
            {
                if (item.Gold > Gold)
                {
                    Console.WriteLine("보유 자금이 부족합니다. 진행하시려면 엔터를 눌러주세요.");
                    Console.ReadLine();
                    return;
                }
                Gold -= item.Gold;
                Inven.AddItem(item);
                Inven.GetItem(Inven.Count-1)?.ValueRef.SetPrice(item.Gold * 85 / 100);
            }
            else
                Console.WriteLine("아이템 정보가 없습니다.");
        }

        public void GetGold(int earning)
        {
            Gold += earning;
        }

        public void GetExp(int exp)
        {
            Exp += exp;
            while (Exp >= 100)
            {
                Exp -= 100;
                LevelUP();
            }
        }

        public void LevelUP()
        {
            TakeHeal(baseHP);
            baseAtk += 0.5f;
            baseDef += 1f;
            ++Level;
        }

        public bool LoseGold(int lose)
        {
            if (Gold < lose)
                return false;
            Gold -= lose;
            return true;
        }
    }
}
