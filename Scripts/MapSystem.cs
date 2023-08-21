using System.Runtime.CompilerServices;

internal partial class TextRPG
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
                    map[i, (int)LocationType.Status] = 1;
                    map[i, (int)LocationType.EquipSetting] = 1;
                    break;
                case LocationType.EquipSetting:
                    map[i, (int)LocationType.Main] = 1;
                    map[i, (int)LocationType.Status] = 1;
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
                    map[i, (int)LocationType.Dungeoning] = 1;
                    break;
                case LocationType.Dungeoning:
                    map[i, (int)LocationType.Dungeon] = 1;
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
        Dungeoning,
        Ending
    }

    public class Location : IDisplay
    {
        public LocationType Type { get; set; }
        private int[,] mMap;
        private Character mPlayer;
        private Store mStore;
        private Dungeon? mDungeon;
        public int Choice { get; private set; }

        public Location(int[,] map, Character player, Store store)
        {
            Type = LocationType.Main;
            mMap = map;
            mPlayer = player;
            mStore = store;
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
                case LocationType.Dungeoning:
                    DisplayDungeoning();
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
                // 엔딩과 던전 진행 중에는 맵 이동은 별도로 실행
                if (Type == LocationType.Ending || Type == LocationType.Dungeoning)
                    continue;
                else if (i == (int)LocationType.Dungeoning)
                    continue;

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
            for (int i = 0; i < route.Length; ++i)
            {
                Console.Write($"[{i}] ");
                switch (route[i])
                {
                    case LocationType.Main:
                        Console.Write("나가기");
                        break;
                    case LocationType.Status:
                        Console.Write("상태창");
                        break;
                    case LocationType.Inventory:
                        Console.Write("인벤토리");
                        break;
                    case LocationType.EquipSetting:
                        Console.Write("장착 관리");
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
                        // Dungeoning에서는 전부 표시 안되므로 삭제함.
                        Console.Write("던전");
                        break;
                    case LocationType.Dungeoning:
                        // Console.Write("던전 진입");
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
            WriteWithColor("상태 보기\n", ConsoleColor.White, ConsoleColor.DarkBlue);
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();
            Choice = mPlayer.Display();
        }
        public void DisplayInventory()
        {
            WriteWithColor("인벤토리\n", ConsoleColor.White, ConsoleColor.DarkGreen);
            Console.WriteLine("보유 중인 아이템입니다.");
            Console.WriteLine();
            var outputString = mPlayer.Inven.GetDisplayString(false);
            foreach (var output in outputString)
            {
                if (output.ToString().Contains("[E]"))
                {
                    var tempStr = output.ToString().Split("[E]");
                    Console.Write(tempStr[0]);
                    WriteWithColor("[E]", ConsoleColor.Yellow);
                    Console.Write(tempStr[1]);
                }
                else
                    Console.Write(output);
            }
        }
        public void DisplayEquipSetting()
        {
            WriteWithColor("인벤토리 - 장착 관리\n", ConsoleColor.White, ConsoleColor.DarkRed);
            Console.WriteLine("보유 중인 아이템을 장착 관리할 수 있습니다.");
            Console.WriteLine();
            var outputString = mPlayer.Inven.GetDisplayString(false);
            int i = GetEnableRoute().Length;
            foreach (var info in outputString)
            {
                info.Insert(0, $"[{i}]");
                ++i;
            }
            Choice = outputString.Length;
            foreach (var output in outputString)
            {
                if (output.ToString().Contains("[E]"))
                {
                    var tempStr = output.ToString().Split("[E]");
                    Console.Write(tempStr[0]);
                    WriteWithColor("[E]", ConsoleColor.Yellow);
                    Console.Write(tempStr[1]);
                }
                else
                    Console.Write(output);
            }
        }
        public void DisplayStoreBuy()
        {
            WriteWithColor("상점\n", ConsoleColor.White, ConsoleColor.DarkMagenta);
            Console.WriteLine("상인에게서 물건을 사고 팔 수 있습니다.");
            Console.WriteLine($"\n[보유 골드]\n{mPlayer.Gold} G\n");
            var outputString = mStore.Inven.GetDisplayString(true);
            int i = GetEnableRoute().Length;
            foreach (var info in outputString)
            {
                info.Insert(0, $"[{i}]");
                ++i;
            }
            Choice = outputString.Length;
            foreach (var output in outputString)
                Console.Write(output);
        }
        public void DisplayStoreSell()
        {
            WriteWithColor("상점\n", ConsoleColor.White, ConsoleColor.DarkYellow);
            Console.WriteLine("상인에게서 물건을 사고 팔 수 있습니다.");
            Console.WriteLine($"\n[보유 골드]\n{mPlayer.Gold} G\n");
            var outputString = mPlayer.Inven.GetDisplayString(true);
            int i = GetEnableRoute().Length;
            foreach (var info in outputString)
            {
                info.Insert(0, $"[{i}]");
                ++i;
            }
            Choice = outputString.Length;
            foreach (var output in outputString)
            {
                if (output.ToString().Contains("[E]"))
                {
                    var tempStr = output.ToString().Split("[E]");
                    Console.Write(tempStr[0]);
                    WriteWithColor("[E]", ConsoleColor.Yellow);
                    Console.Write(tempStr[1]);
                }
                else
                    Console.Write(output);
            }
        }
        public void DisplayDungeon()
        {
            WriteWithColor("던전\n", ConsoleColor.Red, ConsoleColor.White);
            Console.WriteLine("던전에 들어갈 수 있습니다.");
            Console.WriteLine();
            WriteWithColor("[1] 던전 진입", ConsoleColor.Red, ConsoleColor.White);
            Choice = 1; // 진입
        }
        public void DisplayDungeoning()
        {
            Console.WriteLine("던전 탐험 진행 중");
            Console.WriteLine();
            // 던전 함수
            if (mDungeon != null)
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
                    mPlayer.BuyItem(mStore.SellToPlayer(i));
                    break;
                case LocationType.StoreSell:
                    // 장비를 판다.
                    mPlayer.SellItem(i);
                    break;
                case LocationType.Dungeon:
                    // 입력에 따른 행동을 한다.
                    if (i == 0)
                    {
                        mDungeon = new Dungeon(mPlayer);
                        ChageLocation(LocationType.Dungeoning);
                    }
                    break;
                case LocationType.Dungeoning:
                    // 입력에 따른 행동을 한다.
                    if (mDungeon != null)
                    {
                        if (mDungeon.ChangeState(i) == false)
                            ChageLocation(LocationType.Dungeon);
                    }
                    else
                        ChageLocation(LocationType.Dungeon);
                    break;
                case LocationType.Ending:

                    break;
                default:
                    break;
            }
        }

        public void WriteWithColor(string cout, ConsoleColor charColor, ConsoleColor backColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = charColor;
            Console.BackgroundColor = backColor;
            Console.Write(cout);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        
        public void WriteWithCustomColor(string cout, int foreColor, int backColor)
        {
            Console.Write("\x1b[38;5;" + foreColor + "\x1b[48;5;" + backColor + $"m{cout}");
        }
    }
}