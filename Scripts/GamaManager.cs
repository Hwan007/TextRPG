using System.Text.Json;

internal partial class TextRPG
{
    public class GamaManager
    {
        private CharacterSystem? sPlayer;
        private Location? sLocate;
        private StoreSystem? sStore;

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
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true, IncludeFields = true };
            var weapons = JsonFileIOStream.LoadFile<Weapon[]>("Weapon.json", options);
            options = new JsonSerializerOptions() { WriteIndented = true, IncludeFields = true };
            var armors = JsonFileIOStream.LoadFile<Armor[]>("Armor.json", options);

            if (JsonFileIOStream.CheckFile("SaveData.json") == false)
            // 캐릭터 정보 세팅
            {
                // 이름 입력할 수 있게 하기.
                // 능력치를 랜덤으로 정하게 하기.
                Console.Write("이름을 입력해주세요 : ");
                string? name = Console.ReadLine();
                if (name != null)
                    sPlayer = new CharacterSystem(name, "전사", 1, 10, 5, 100, 1500);
                else
                    sPlayer = new CharacterSystem("철수", "전사", 1, 10, 5, 100, 1500);
            }
            else
            {
                CharacterConverter converter = new CharacterConverter();
                options = new JsonSerializerOptions() { WriteIndented = true, IncludeFields = true };
                options.Converters.Add(converter);
                var save = JsonFileIOStream.LoadFile<CharacterSystem>("SaveData.json", options);
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
                sStore = new StoreSystem();

                sStore.AddItems(weapons);
                sStore.AddItems(armors);

                // 맵 연결 정보를 가져오기
                // map은 LocationType의 최대개수 정사각행렬
                int[,] map = Location.MapSetting();
                sLocate = new Location(map, sPlayer, sStore);
            }
        }

        public void RunGame()
        {
            if (sPlayer is CharacterSystem && sLocate is Location)
            {
                while (true)
                {
                    // 주요 로직으로 while으로 State에 따라서 Display를 한다.

                    // 화면 표시
                    sLocate.SetDisplayString();
                    var route = sLocate.AddEnableRouteToSBList();
                    DisplaySystem.DisplayOut();
                    // 입력을 기다린다.
                    bool IsValidInput = false;
                    while (IsValidInput == false)
                    {
                        Console.WriteLine("\n원하시는 행동을 입력해주세요");
                        WriteWithCustomColor(">> ", 166);
                        var input = Console.ReadLine();
                        // 입력을 받고 위치를 바꾸거나 행동의 취한다.
                        if (input is string)
                        {
                            if (int.TryParse(input, out var id))
                            {
                                if (id < route.Length && id >= 0)
                                {
                                    // 상황에 맞는 동작을 해야 한다.
                                    sLocate.ChageLocation(route[id]);
                                    IsValidInput = true;
                                }
                                else
                                {
                                    if (id - route.Length < sLocate.Choice && id >= 0)
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
    }
}
