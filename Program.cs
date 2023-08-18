using System.Data;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    static void Main(string[] args)
    {
        TextRPG game = new TextRPG();
        game.GameStart();

        //JsonFileIOStream stream = new JsonFileIOStream();
        //stream.TestSave();
        //stream.TestLoad();
    }
}

internal class TextRPG
{
    private Character? sPlayer;
    private Location? sLocate;
    private Store? sStore;
    private Dungeon? sDungeon;

    public void GameStart()
    {
        GameDataSetting();
        RunGame();
    }
    public void GameDataSetting()
    {
        // 저장된 파일이 있는지 확인
        // 저장된 파일이 있는 경우에 가져오기

        // 캐릭터 정보 세팅
        sPlayer = new Character("Chad", "전사", 1, 10, 5, 100, 1500);

        // 아이템 정보 세팅
        // Json으로 된 아이템 정보 가져오기

        // 아이템 리스트를 스토어에 넣어서 스토어가 가지고 있게 하기
        sStore = new Store();
        sDungeon = new Dungeon();

        // 맵 연결 정보를 가져오기
        // map은 LocationType의 최대개수 정사각행렬
        int[,] map = MapSetting();
        sLocate = new Location(map, sPlayer, sStore, sDungeon);
    }

    /// <summary>
    /// 미리 정의한 이동 가능한 경로입니다.
    /// </summary>
    /// <returns></returns>
    public int[,] MapSetting()
    {
        int[,] map = new int[(int)LocationType.Ending + 1, (int)LocationType.Ending + 1];
        for (int i = 0; i <= (int)LocationType.Ending; ++i)
        {
            switch ((LocationType)i)
            {
                case LocationType.Main:
                    map[i, (int)LocationType.Status] = 1;
                    map[i, (int)LocationType.Inventory] = 1;
                    map[i, (int)LocationType.StoreBuy] = 1;
                    map[i, (int)LocationType.Dungeon] = 1;
                    break;
                case LocationType.Status:
                    map[i, (int)LocationType.Main] = 1;
                    map[i, (int)LocationType.Inventory] = 1;
                    map[i, (int)LocationType.EquipSetting] = 1;
                    break;
                case LocationType.Inventory:
                    map[i, (int)LocationType.Main] = 1;
                    map[i, (int)LocationType.EquipSetting] = 1;
                    break;
                case LocationType.EquipSetting:
                    map[i, (int)LocationType.Main] = 1;
                    map[i, (int)LocationType.Inventory] = 1;
                    break;
                case LocationType.StoreBuy:
                    map[i, (int)LocationType.Main] = 1;
                    map[i, (int)LocationType.StoreSell] = 1;
                    break;
                case LocationType.StoreSell:
                    map[i, (int)LocationType.Main] = 1;
                    map[i, (int)LocationType.StoreBuy] = 1;
                    break;
                case LocationType.Dungeon:
                    map[i, (int)LocationType.Main] = 1;
                    map[i, (int)LocationType.Status] = 1;
                    break;
                case LocationType.Ending:
                    break;
                default:
                    break;
            }
        }
        return map;
    }

    public void RunGame()
    {
        if (sPlayer is Character && sLocate is Location)
        {
            while (sPlayer.IsDead == false)
            {
                // 주요 로직으로 while으로 State에 따라서 Display를 한다.

                // 화면 표시
                sLocate.Display();
                var route = sLocate.DisplayEnableRoute();
                // 입력을 기다린다.
                var input = Console.ReadLine();
                // 입력을 받고 위치를 바꾸거나 행동의 취한다.
                if (input is string)
                {
                    if (int.TryParse(input, out var id))
                    {
                        if (id < sLocate.Choice)
                        {
                            // 상황에 맞는 동작을 해야 한다.
                            sLocate.ActByInput(id);
                        }
                        else
                        {
                            sLocate.ChageLocation(route[id - sLocate.Choice]);
                        }
                    }
                }
            }
            // 죽으면 while을 빠져나온다.
            // 다시 시작하시겠습니까?를 물어봐서 다시 시작하도록 한다.
        }
    }

    public enum LocationType
    {
        Main,
        Status,
        Inventory,
        EquipSetting,
        StoreBuy,
        StoreSell,
        Dungeon,
        Ending
    }

    public class Location : IDisplay
    {
        public LocationType Type { get; set; }
        private int[,] mMap;
        private Character mPlayer;
        private Store mStore;
        private Dungeon mDungeon;
        public int Choice { get; private set; }

        public Location(int[,] map, Character player, Store store, Dungeon dungeon)
        {
            Type = LocationType.Main;
            mMap = map;
            mPlayer = player;
            mStore = store;
            mDungeon = dungeon;
        }
        public int Display()
        {
            Choice = 0;
            Console.Clear();
            switch (Type)
            {
                case LocationType.Main:
                    DisplayMain();
                    break;
                case LocationType.Status:
                    DisplayStatus();
                    break;
                case LocationType.Inventory:
                    DisplayInventory();
                    break;
                case LocationType.EquipSetting:
                    DisplayEquipSetting();
                    break;
                case LocationType.StoreBuy:
                    DisplayStoreBuy();
                    break;
                case LocationType.StoreSell:
                    DisplayStoreSell();
                    break;
                case LocationType.Dungeon:
                    DisplayDungeon();
                    break;
                case LocationType.Ending:
                    DisplayEnding();
                    break;
                default:
                    break;
            }
            return 0;
        }

        public bool ChageLocation(LocationType type)
        {
            if (mMap[(int)Type, (int)type] == 1)
            {
                Type = type;
                return true;
            }
            else
                return false;
        }

        public LocationType[] GetEnableRoute()
        {
            List<LocationType> route = new List<LocationType>();
            for (int i = 0; i <= (int)LocationType.Ending; ++i)
            {
                if (mMap[(int)Type, i] == 1)
                {
                    route.Add((LocationType)i);
                }
            }
            return route.ToArray();
        }

        public LocationType[] DisplayEnableRoute()
        {
            Console.WriteLine();
            var route = GetEnableRoute();
            for (int i = Choice; i < Choice + route.Length; ++i)
            {
                Console.Write($"[{i}] ");
                switch (route[i - Choice])
                {
                    case LocationType.Main:
                        Console.Write("마을");
                        break;
                    case LocationType.Status:
                        Console.Write("상태창");
                        break;
                    case LocationType.Inventory:
                        Console.Write("인벤토리");
                        break;
                    case LocationType.EquipSetting:
                        Console.Write("장비관리");
                        break;
                    case LocationType.StoreBuy:
                        if (Type == LocationType.StoreSell)
                            Console.Write("구매");
                        else
                            Console.Write("상점");
                        break;
                    case LocationType.StoreSell:
                        Console.Write("판매");
                        break;
                    case LocationType.Dungeon:
                        Console.Write("던전");
                        break;
                    case LocationType.Ending:
                        Console.Write("Game Over");
                        break;
                    default:
                        break;
                }
                Console.Write("\n");
            }
            return route;
        }

        public void DisplayMain()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
        }
        public void DisplayStatus()
        {
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();
            Choice = mPlayer.Display();
        }
        public void DisplayInventory()
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Choice = mPlayer.Inven.Display();
        }
        public void DisplayEquipSetting()
        {
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Choice = mPlayer.Inven.Display();
        }
        public void DisplayStoreBuy()
        {
            Console.WriteLine("상점");
            Console.WriteLine("상인에게서 물건을 사고 팔 수 있습니다.");
            Console.WriteLine();
            Choice = mStore.Inven.Display();
        }
        public void DisplayStoreSell()
        {
            Console.WriteLine("상점");
            Console.WriteLine("상인에게서 물건을 사고 팔 수 있습니다.");
            Console.WriteLine();
            Choice = mPlayer.Inven.Display();
        }
        public void DisplayDungeon()
        {
            Console.WriteLine("던전");
            Console.WriteLine("던전에 들어갈 수 있습니다.");
            Console.WriteLine();
            // 던전 함수
            Choice = mDungeon.Display();
        }
        public void DisplayEnding()
        {
            Console.WriteLine("엔딩");
            Console.WriteLine("Created by Hwan007");
            Console.WriteLine();
        }

        public void ActByInput(int i)
        {
            switch (Type)
            {
                case LocationType.Main:

                    break;
                case LocationType.Status:

                    break;
                case LocationType.Inventory:

                    break;
                case LocationType.EquipSetting:
                    // 장비를 착용
                    //mPlayer.Inven.GetItem(i).ValueRef;
                    break;
                case LocationType.StoreBuy:
                    // 장비를 산다.
                    break;
                case LocationType.StoreSell:
                    // 장비를 판다.
                    break;
                case LocationType.Dungeon:
                    // 던전을 들어간다.
                    break;
                case LocationType.Ending:

                    break;
                default:
                    break;
            }
        }
    }


    /// <summary>
    /// 화면에 뿌리는 내용만 담당하는 클래스
    /// </summary>
    public interface IDisplay
    {
        public int Display();
    }

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
        public int Gold { get; }
        public Inventory Inven { get; }
        public bool IsDead { get => Hp <= 0 ? true : false; }
        private Item[] Equipments;

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
            Equipments = new Item[2];
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

        public bool EquipItem(Item item)
        {
            if (item.Data.type == EquipType.Weapon)
            {
                var weapon = item as Weapon;
                if (weapon != null)
                {// 장착된 장비가 있는 경우에 벗는다.
                    if (Equipments[(int)EquipType.Weapon] != null)
                        Equipments[(int)EquipType.Weapon].IsEquip = false;
                    // 장비를 장착한다.
                    Equipments[(int)EquipType.Weapon] = weapon;
                    weapon.IsEquip = true;
                    return true;
                }
            }
            else if (item is Armor)
            {
                var armor = item as Armor;
                if (armor != null)
                {// 장착된 장비가 있는 경우에 벗는다.
                    if (Equipments[(int)EquipType.Armor] != null)
                        Equipments[(int)EquipType.Armor].IsEquip = false;
                    // 장비를 장착한다.
                    Equipments[(int)EquipType.Armor] = armor;
                    armor.IsEquip = true;
                    return true;
                }
            }
            // 허용된 타입이 아니거나, 정상적으로 진행되지 못했다.
            return false;
        }

        public void UnEquipItem(Item item)
        {
            if (item.Data.type == EquipType.Weapon)
            {
                var weapon = item as Weapon;
                if (weapon != null)
                {// 장착된 장비가 있는 경우에 벗는다.
                    if (Equipments[(int)EquipType.Weapon] != null)
                        Equipments[(int)EquipType.Weapon].IsEquip = false;
                }
            }
            else if (item is Armor)
            {
                var armor = item as Armor;
                if (armor != null)
                {// 장착된 장비가 있는 경우에 벗는다.
                    if (Equipments[(int)EquipType.Armor] != null)
                        Equipments[(int)EquipType.Armor].IsEquip = false;
                }
            }
            // 허용된 타입이 아니거나, 정상적으로 진행되지 못했다.
        }
    }

    public class Inventory : IDisplay
    {
        private LinkedList<Item> mItems;

        public Inventory()
        {
            mItems = new LinkedList<Item>();
        }
        public int Display()
        {
            Console.WriteLine("[아이템 목록]");
            // 인벤토리 내용물 표시
            int i = 0;
            foreach (Item item in mItems)
            {
                Console.Write($"- {i} ");
                item.Display();
                Console.Write("\n");
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

    public class Store : IDisplay
    {
        public Inventory Inven { get; }

        public Store()
        {
            Inven = new Inventory();
        }
        public int Display()
        {
            return 1;
        }
    }

    public class Dungeon : IDisplay
    {
        public int Display()
        {
            return 1;
        }
    }
}
