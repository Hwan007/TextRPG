using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{

    static void Main(string[] args)
    {
        //TextRPG game = new TextRPG();
        //game.GameStart();

        JsonFileIOStream JsonIO = new JsonFileIOStream();
        
    }
}

internal partial class TextRPG
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

    /// <summary>
    /// 화면에 뿌리는 내용만 담당하는 클래스
    /// </summary>
    public interface IDisplay
    {
        public int Display();
    }
}
