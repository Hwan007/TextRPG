using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetConsoleMode(IntPtr handle, out int mode);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int handle);

    static void Main(string[] args)
    {
        var handle = GetStdHandle(-11);
        int mode;
        GetConsoleMode(handle, out mode);
        // You need set flag ENABLE_VIRTUAL_TERMINAL_PROCESSING(0x4) by SetConsoleMode
        SetConsoleMode(handle, mode | 0x4);

        for (int i = 0; i < 255; i++)
        {
            //Console.Write("\x1b[38;5;" + i + "m" + "\x1b[48;5;" + i + $"m■{i}");
            //Console.Write("\x1b[38;5;" + i + $"m");
            //Console.Write("\x1b[48;5;" + i + $"m■{i}\t");
            //Console.Write("\x1b[38;5;" + i + $"m■{i}\t");
            //Console.Write("\x1b[48;5;" + i + $"m■{i}\t");
        }

        Console.ReadLine();

        //for (int i = 0; i <= (int)ConsoleColor.White; ++i)
        //{
        //    if (i < 6)
        //        Console.BackgroundColor = ConsoleColor.Gray;
        //    else
        //        Console.BackgroundColor = ConsoleColor.Black;
        //    Console.ForegroundColor = (ConsoleColor)i;
        //    Console.WriteLine("{0} {1} 글씨색깔을 미리보기 위한 테스트입니다.\n", i, (ConsoleColor)i);
        //}
        //for (int i = 0; i <= (int)ConsoleColor.White; ++i)
        //{
        //    if (i < 7)
        //        Console.ForegroundColor = ConsoleColor.White;
        //    else
        //        Console.ForegroundColor = ConsoleColor.Black;
        //    Console.BackgroundColor = (ConsoleColor)i;
        //    Console.WriteLine("{0} {1} 배경색을 미리보기 위한 테스트입니다.\n", i, (ConsoleColor)i);
        //}
        //Console.ForegroundColor = ConsoleColor.White;
        //Console.BackgroundColor = ConsoleColor.Black;

        //JsonFileIOStream JsonIO = new JsonFileIOStream();
        //JsonIO.SaveItemDataBase();

        //TextRPG game = new TextRPG();
        //game.GameStart();
    }
}

internal partial class TextRPG
{
    private Character? sPlayer;
    private Location? sLocate;
    private Store? sStore;
    private JsonFileIOStream? sJsonIO;

    public void GameStart()
    {
        GameDataSetting();
        RunGame();
    }
    public void GameDataSetting()
    {
        // 저장된 파일이 있는지 확인
        // 저장된 파일이 있는 경우에 가져오기
        // Json으로 된 아이템 정보 가져오기
        sJsonIO = new JsonFileIOStream();
        var weapons = sJsonIO.LoadFile<Weapon[]>("Weapon.json");
        var armors = sJsonIO.LoadFile<Armor[]>("Armor.json");

        if (sJsonIO.CheckFile("SaveData.json") == false)
        // 캐릭터 정보 세팅
        {
            // 이름 입력할 수 있게 하기.
            // 능력치를 랜덤으로 정하게 하기.
            sPlayer = new Character("철수", "전사", 1, 10, 5, 100, 1500);
        }
        else
        {
            var save = sJsonIO.LoadFile<Character>("SaveData.json");
            sPlayer = save;
        }

        // 아이템 정보 세팅
        // 1번째 아이템은 플래이어에 인벤토리에 넣고 장착시키기
        if (sPlayer != null && weapons != null && armors != null)
        {
            //sPlayer.Inven.AddItem(weapons[0]);
            //sPlayer.Inven.AddItem(armors[0]);
            sPlayer.GetGold(weapons[0].Gold);
            sPlayer.GetGold(armors[0].Gold);
            sPlayer.BuyItem(weapons[0]);
            sPlayer.BuyItem(armors[0]);
            sPlayer.Inven.GetItem(0)?.Value.EquipByCharacter(sPlayer);
            sPlayer.Inven.GetItem(1)?.Value.EquipByCharacter(sPlayer);

            // 나머지는 스토어에 넣기


            // 아이템 리스트를 스토어에 넣어서 스토어가 가지고 있게 하기
            sStore = new Store();

            sStore.AddItems(weapons);
            sStore.AddItems(armors);

            // 맵 연결 정보를 가져오기
            // map은 LocationType의 최대개수 정사각행렬
            int[,] map = MapSetting();
            sLocate = new Location(map, sPlayer, sStore);
        }
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
                bool IsValidInput = false;
                while (IsValidInput == false)
                {
                    var input = Console.ReadLine();
                    // 입력을 받고 위치를 바꾸거나 행동의 취한다.
                    if (input is string)
                    {
                        if (int.TryParse(input, out var id))
                        {
                            if (id < route.Length)
                            {
                                // 상황에 맞는 동작을 해야 한다.
                                sLocate.ChageLocation(route[id]);
                                IsValidInput = true;
                            }
                            else
                            {
                                if (id - route.Length < sLocate.Choice)
                                {
                                    sLocate.ActByInput(id - route.Length);
                                    IsValidInput = true;
                                }
                                else
                                    Console.WriteLine("잘못된 입력입니다.");
                            }
                        }
                    }
                }
            }
            // 죽으면 while을 빠져나온다.
            // 다시 시작하시겠습니까?를 물어봐서 다시 시작하도록 한다.
        }
    }

    /// <summary>
    /// 화면에 뿌리는 함수 표현 인터페이스
    /// </summary>
    public interface IDisplay
    {
        public int Display();
    }
}
