using System.Text;
using System.Text.Json;

internal partial class TextRPG
{
    public enum LocationType
    {
        Main,
        Status,
        Inventory,
        EquipSetting,
        StoreBuy,
        StoreSell,
        RestForHeal,
        Dungeon,
        Dungeoning,
        Ending
    }

    public class MapSystem : DisplaySystem
    {
        public LocationType Type { get; set; }
        private int[,] mMap;
        private CharacterSystem mPlayer;
        private StoreSystem mStore;
        private DungeonSystem? mDungeon;
        public int Choice { get; private set; }

        public MapSystem(int[,] map, CharacterSystem player, StoreSystem store)
        {
            Type = LocationType.Main;
            mMap = map;
            mPlayer = player;
            mStore = store;
        }
        public override int SetDisplayString()
        {
            Choice = 0;
            DisplaySystem.DisplayClear();
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
                case LocationType.RestForHeal:
                    DisplayRestForHeal();
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

        public LocationType[] AddEnableRouteToSBList()
        {
            AddStringToDisplayList("\n");
            StringBuilder sb = new StringBuilder();
            var route = GetEnableRoute();
            for (int i = 0; i < route.Length; ++i)
            {
                sb.Append($"[{i}] ");
                switch (route[i])
                {
                    case LocationType.Main:
                        sb.Append("나가기");
                        break;
                    case LocationType.Status:
                        sb.Append("상태창");
                        break;
                    case LocationType.Inventory:
                        sb.Append("인벤토리");
                        break;
                    case LocationType.EquipSetting:
                        sb.Append("장착 관리");
                        break;
                    case LocationType.StoreBuy:
                        if (Type == LocationType.StoreSell)
                            sb.Append("구매");
                        else
                            sb.Append("상점");
                        break;
                    case LocationType.StoreSell:
                        sb.Append("판매");
                        break;
                    case LocationType.RestForHeal:
                        sb.Append("휴식하기");
                        break;
                    case LocationType.Dungeon:
                        // Dungeoning에서는 전부 표시 안되므로 삭제함.
                        sb.Append("던전");
                        break;
                    case LocationType.Dungeoning:
                        // Console.Write("던전 진입");
                        break;
                    case LocationType.Ending:
                        sb.Append("Game Over");
                        break;
                    default:
                        break;
                }
                sb.Append("\n");
                AddStringToDisplayList(sb.ToString());
                sb.Clear();
            }
            return route;
        }

        public void DisplayMain()
        {
            AddStringToDisplayList("스파르타 마을에 오신 여러분 환영합니다.\n");
            AddStringToDisplayList("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
        }
        public void DisplayStatus()
        {
            AddStringToDisplayList(StringWithCustomColor("상태 보기\n", ColorType.PurpleBlue));
            AddStringToDisplayList("캐릭터의 정보가 표시됩니다.\n");
            Choice = mPlayer.SetDisplayString();
        }
        public void DisplayInventory()
        {
            AddStringToDisplayList(StringWithCustomColor("인벤토리\n", ColorType.Orange));
            AddStringToDisplayList("보유 중인 아이템입니다.\n");
            StringBuilder[] outputString = mPlayer.Inven.GetDisplayString(false);
            foreach (StringBuilder output in outputString)
            {
                if (output.ToString().Contains("[E]"))
                {
                    var tempStr = output.ToString().Split("[E]");
                    AddStringToDisplayList(tempStr[0] + StringWithCustomColor("[E]", 220, 0) + tempStr[1]);
                }
                else
                    AddStringToDisplayList(output.ToString());
            }
        }
        public void DisplayEquipSetting()
        {
            AddStringToDisplayList(StringWithCustomColor("인벤토리", ColorType.Orange));
            AddStringToDisplayList(StringWithCustomColor(" - 장착 관리\n", ColorType.Gray));
            AddStringToDisplayList("보유 중인 아이템을 장착 관리할 수 있습니다.\n");
            StringBuilder[] outputString = mPlayer.Inven.GetDisplayString(false);
            Choice = outputString.Length;
            int i = GetEnableRoute().Length;
            foreach (StringBuilder output in outputString)
            {
                output.Insert(0, $"[{i}]{(i >= 10 ? "" : " ")}");
                ++i;

                if (output.ToString().Contains("[E]"))
                {
                    var tempStr = output.ToString().Split("[E]");
                    AddStringToDisplayList(tempStr[0] + StringWithCustomColor("[E]", 220, 0) + tempStr[1]);
                }
                else
                    AddStringToDisplayList(output.ToString());
            }
        }
        public void DisplayStoreBuy()
        {
            AddStringToDisplayList(StringWithCustomColor("상점\n", 83, 0));
            AddStringToDisplayList("상인에게서 물건을 사고 팔 수 있습니다.\n");
            AddStringToDisplayList(StringWithCustomColor("\n[보유 골드]"));
            AddStringToDisplayList(StringWithCustomColor($"\n{mPlayer.Gold:N0} G\n\n", ColorType.Gold));
            StringBuilder[] outputString = mStore.Inven.GetDisplayString(true);
            Choice = outputString.Length;
            
            int i = GetEnableRoute().Length;
            foreach (StringBuilder info in outputString)
            {
                info.Insert(0, $"[{i}]{(i >= 10 ? "" : " ")}");
                ++i;
                AddStringToDisplayList(info.ToString());
            }
        }
        public void DisplayStoreSell()
        {
            AddStringToDisplayList(StringWithCustomColor("상점\n", 83, 0));
            AddStringToDisplayList("상인에게서 물건을 사고 팔 수 있습니다.\n");
            AddStringToDisplayList(StringWithCustomColor("\n[보유 골드]"));
            AddStringToDisplayList(StringWithCustomColor($"\n{mPlayer.Gold:N0} G\n\n", ColorType.Gold));            
            var outputString = mPlayer.Inven.GetDisplayString(true);
            Choice = outputString.Length;

            int i = GetEnableRoute().Length;
            foreach (StringBuilder output in outputString)
            {
                output.Insert(0, $"[{i}]{(i >= 10 ? "" : " ")}");
                ++i;

                if (output.ToString().Contains("[E]"))
                {
                    var tempStr = output.ToString().Split("[E]");
                    AddStringToDisplayList(tempStr[0] + StringWithCustomColor("[E]", 220, 0) + tempStr[1]);
                }
                else
                    AddStringToDisplayList(output.ToString());
            }
        }
        public void DisplayRestForHeal()
        {
            AddStringToDisplayList(StringWithCustomColor("휴식하기\n", ColorType.PurpleBlue));
            AddStringToDisplayList(StringWithCustomColor("500 G", ColorType.Gold) + "를 내면 체력을 회복할 수 있습니다. (보유 골드 : " +
                                   StringWithCustomColor($"{mPlayer.Gold} G", ColorType.Gold) + ")\n\n");
            AddStringToDisplayList("[1] 휴식하기\n");
            Choice = 1;
        }
        public void DisplayDungeon()
        {
            AddStringToDisplayList(StringWithCustomColor("던전\n", ColorType.Red, 0));
            AddStringToDisplayList("던전에 들어갈 수 있습니다.\n\n");   
            AddStringToDisplayList(StringWithCustomColor("[1] 던전 진입\n", ColorType.Red, 0));
            Choice = 1; // 진입
        }
        public void DisplayDungeoning()
        {
            AddStringToDisplayList("던전 탐험 진행 중\n\n");
            // 던전 함수
            if (mDungeon != null)
                Choice = mDungeon.SetDisplayString();
        }
        public void DisplayEnding()
        {
            AddStringToDisplayList("엔딩\n");
            AddStringToDisplayList("Created by Hwan007");
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
                case LocationType.RestForHeal:
                    if (mPlayer.LoseGold(500))
                    {
                        mPlayer.TakeHeal(1000);
                        Console.Write("체력을 회복하였습니다.\n");
                        Thread.Sleep(1000);
                        CharacterConverter converter = new CharacterConverter();
                        JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true, IncludeFields = true };
                        options.Converters.Add(converter);
                        JsonFileIOStream.SaveFile<CharacterSystem>("SaveData.json", mPlayer, options);
                        Console.Write("저장하였습니다.\n");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.Write("금액이 부족합니다.\n");
                        Thread.Sleep(1000);
                    }
                    break;
                case LocationType.Dungeon:
                    // 입력에 따른 행동을 한다.
                    if (i == 0)
                    {
                        mDungeon = new DungeonSystem(mPlayer);
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

        /// <summary>
        /// 미리 정의한 이동 가능한 경로입니다.
        /// </summary>
        /// <returns></returns>
        public static int[,] MapSetting()
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
                        map[i, (int)LocationType.RestForHeal] = 1;
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
                    case LocationType.RestForHeal:
                        map[i, (int)LocationType.Main] = 1;
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
    }
}