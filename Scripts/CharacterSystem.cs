using System.Text;
using System.Text.Json.Serialization;

internal partial class TextRPG
{
    public class CharacterSystem : DisplaySystem
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
        public InventorySystem Inven { get; }
        public bool IsDead { get => Hp <= 0 ? true : false; }
        public EquipmentSystem Equipments { get; }
        public int Exp { get; private set; }

        [JsonConstructor]
        public CharacterSystem(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            baseAtk = atk;
            baseDef = def;
            baseHP = Hp= hp;
            Gold = gold;
            Inven = new InventorySystem();
            Equipments = new EquipmentSystem();
        }

        public override int SetDisplayString()
        {
            int start = AddStringToDisplayList(StringWithCustomColor(" ===================\n"));
            AddStringToDisplayList(StringWithCustomColor($" Lv.{Level}\n"));
            AddStringToDisplayList(StringWithCustomColor($" {Name, -4} ({Job})\n"));
            AddStringToDisplayList(StringWithCustomColor($" {"체력",-6}   : {Hp}/{baseHP}\n"));
            float tmpAtk = Atk;
            AddStringToDisplayList(StringWithCustomColor($" {"공격력",-7} : {tmpAtk} {(tmpAtk > baseAtk ? $" (+{tmpAtk - baseAtk})" : "")}\n"));
            float tmpDef = Def;
            AddStringToDisplayList(StringWithCustomColor($" {"방어력",-7} : {tmpDef} {(tmpDef > baseDef ? $" (+{tmpDef - baseDef})" : "")}\n"));
            AddStringToDisplayList(StringWithCustomColor($" {"골드",-6}   : "));
            AddStringToDisplayList(StringWithCustomColor($"{Gold:N0} G\n", 178));
            int end = AddStringToDisplayList(StringWithCustomColor(" ===================\n"));
            return end - start;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                damage = 1;
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
