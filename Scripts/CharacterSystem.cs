﻿internal partial class TextRPG
{
    public class CharacterSystem : DisplaySystem
    {
        public string Name { get; private set; }
        public string Job { get; private set; }
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
        public float baseAtk { get; private set; }
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
        public float baseDef { get; private set; }
        public int Hp { get; private set; }
        public int baseHp { get; private set; }
        public int Gold { get; private set; }
        public InventorySystem Inven { get; private set; }
        public bool IsDead { get => Hp <= 0 ? true : false; }
        public EquipmentSystem Equipments { get; private set; }
        public int Exp { get; private set; }

        public CharacterSystem(string name, string job, int level, float baseatk, float basedef, int basehp, int gold, int exp, int hp, InventorySystem inven, EquipmentSystem equipments)
        {
            Name = name;
            Job = job;
            Level = level;
            baseAtk = baseatk;
            baseDef = basedef;
            baseHp = basehp;
            Hp = hp;
            Gold = gold;
            Exp = exp;
            Inven = inven;
            Equipments = equipments;
        }

        public CharacterSystem(string name, string job, int level, float baseatk, float basedef, int basehp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            baseAtk = baseatk;
            baseDef = basedef;
            baseHp = Hp = basehp;
            Inven = new InventorySystem();
            Equipments = new EquipmentSystem();
            Gold = gold;
            Exp = 0;
        }

        public override int SetDisplayString()
        {
            int start = AddStringToDisplayList(StringWithCustomColor(" ===================\n"));
            AddStringToDisplayList(StringWithCustomColor($" Lv.{Level}\n"));
            AddStringToDisplayList(StringWithCustomColor($" {Name, -4} ({Job})\n"));
            AddStringToDisplayList(StringWithCustomColor($" {"체력",-6}   : {Hp}/{baseHp}\n"));
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
            Hp = Hp + heal >= baseHp ? baseHp : Hp + heal;
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

        public void GetGold(int? earning)
        {
            Gold += earning??0;
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
            TakeHeal(baseHp);
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
