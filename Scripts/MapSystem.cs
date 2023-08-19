﻿internal partial class TextRPG
{
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
                    // inventory의 i번째 아이템을 착용
                    mPlayer.Inven.GetItem(i)?.ValueRef.EquipByCharacter(mPlayer);
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
}