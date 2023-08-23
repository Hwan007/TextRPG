# TextRPG

## 구현한 기능 중 일부
1. 시작 시에 이름 입력하여 플래이어 이름 적용된다.
2. 상태창에 플래이어가 장착한 아이템의 정보가 적용된다.
3. 상점에서 아이템을 사고 팔 수 있다.
4. 인벤토리에서 아이템을 장착/탈착 할 수 있다. (정렬하는 기능은 저장하는 기능에 많은 시간을 쏟은 나머지 할 수 없었다.)
5. 휴식하기에서 휴식을 하면 게임이 저장되고, HP가 전부 찬다.
6. 던전에 들어가 돈을 벌 수 있다.
7. 던전에서 경험치를 100 얻게 되면 레벨업을 한다.
8. 던전에서 받는 데미지는 방어력에 의해 깎이고, 던전에서 얻는 골드는 공격력에 의해 증폭된다.
9. 낮은 확률로 던전에서 회복할 수 있다.
 
## 기능(클래스) 별 설명

### << **DisplaySystem** >>

Program.cs 파일에 있으며, 화면에 내보낼 문자열을 Static 변수인 List<string> SBList에 넣거나, 요소들을 꺼내 화면에 출력합니다.

화면에 내보낼 정보를 가지고 있으면 이 클래스를 상속받아 메소드를 구현하거나, static 메소드인 AddStringToDisplayList를 이용합니다.

=> 인터페이스를 이용하려고 했으나, 각 클래스별로 bool 인수를 받고 싶거나, 모든 문자열을 받아 최종적으로 수정할 필요가 있어 이 클래스를 작성하게 되었습니다.

<br>

### << **CharacterSystem** >>

CharacterSystem.cs 파일에 있으며, 플래이어가 가지고 있는 정보를 뜻합니다.

아쉬운 점은 Atk, Def를 가져갈 때마다 체크하도록 한 점입니다. Equipments나 아이템에 캐릭터를 인수로 받아 정보를 세팅하는 함수로 작성하는 것이 나은 것 같습니다.

Json 파일 저장 기능에 가장 많은 시간을 할애하게된 이유가 바로 이 캐릭터 아래 인벤토리 아래 리스트를 가지고 있다는 점이었습니다.
 리스트를 가진 클래스를 객체로 가진 클래스를 Deserialize하려고 할때마다 에러가 발생하였고, Json.Text.Json에서는 LinkedList나 List나 모두 Array 형태로 작성을 하는 것을 알고,
 별도의 JsonConverter를 작성하는 수밖에 없었습니다.

 <br>

### << **DungeonSystem** >>

DungeonSystem.cs 파일에 있으며, 던전 맵에 대한 출력 및 입력에 대한 반응을 담당합니다.

내부에 Stage 클래스가 표현되어 있으며, 각 난이도에 따른 Stage 객체를 생성해서 게임을 진행하고, 던전에 난이도를 선택할 때마다 새로운 객체를 생성합니다.

<br>

### << **EquipmentSystem** >>

EquipmentSystem.cs 파일에 있으며, 플래이어에게 장착 아이템에 대한 정보를 가지고 있는 기능을 더하기 위한 클래스입니다.

장착될 아이템에 대한 LinkedList를 가지고 있으며, 장착/탈착에 대한 기능을 구현한 메소드가 작성되어 있습니다.

※ dynamic을 사용한 것이 왠만한 버그의 원인이었습니다. 클래스를 명확히 명시하는 것이 무엇보다 중요하다는 것을 느꼈습니다.

<br>

### << **InventorySystem** >>

InventorySystem.cs 파일에 있으며, 플래이어 및 상인에게 가지고 있는 아이템들에 대한 정보를 뜻합니다.

가지고 있는 아이템에 대한 LinkedList를 가지고 있으며, 아이템에 대한 정보를 문자열로 반환하거나, 인덱스에 따라서 아이템을 추가하거나 반환하거나 제거합니다.

<br>

### << **ItemSystem** >>

Item, Weapon, Armor 클래스를 총칭합니다. ItemSystem.cs의 파일에 있으며, 아이템 타입에 대한 Enum을 정의하건, 아이템 정보에 대한 구조체가 정의되어 있습니다.

Json으로 저장하는 것에 많은 어려움을 겪게된 이유가 private로 된 필드 멤버가 기본적으로 포함하지 않는 것이었습니다. 시간이 얼마 없어 pulic 속성으로 get만 가능하도록 바꾸어 해결하였지만, 다음번에는 필요한 내용만 저장하려고 합니다.

<br>

### << **JsonFileIOStream** >>

JsonFileIOStream.cs 파일에 있으며, Json 파일에 대한 저장/불러오기 기능을 담당합니다. 어디서나 접근할 수 있도록 static으로 선언되어 있습니다.

가장 많은 시간을 투자한 기능으로, System.Text.Json에 대한 메소드가 익숙하지 않은 점과 Json의 자료구조가 어떻게 구성되었는지 몰랐던 점이 어려운 이유였습니다.

CharacterSystem에 대한 JsonConverter인 CharacterConverter 클래스가 선언되어 있습니다. 다른 이의 코드를 참고하며 작성하였지만, Json 파일의 자료구조가 Tree인 점과 Dictionary로 취급가능하다는 것을 배우게 되었습니다.

<br>

### << **MapSystem** >>

MaySystem.cs 파일에 있으며, 플래이어의 위치에 대한 정보와 위치에 따른 화면 출력을 담당합니다.

Map은 방향성이 없는 2차원 그래프로 작성되어 있습니다. 장소에 대한 Enum을 선언하여 MapSetting에서 어디로 갈 수 있는지 추가하게 됩니다.

<br>

GameManger에 작성된 순서와 밀접한 연관이 있습니다. 다음과 같은 순으로 작동합니다.

```
int[,] map = MapSystem.MapSetting();
sLocate = new MapSystem(map, sPlayer, sStore);
```

우선 게임 매니저에서 Map 세팅을 합니다.


```SetDisplayString() -> Display****()```

① 플래이어 위치에 대하여 머리글을 SBList에 넣습니다.

② 위치에 대하여 필요한 정보를 SBList에 넣도록 각 객체에게 요청합니다.

③ 이때 각 객체에서 돌려준 선택지의 개수가 몇개인지 체크합니다.

(필요한 경우에 string[]마다 앞에 번호를 넣습니다.)

([E]에 대한 코드는 수정할 필요가 있습니다. 여기서할 것이 아니라 내부에서 작성하는 것으로 충분합니다.)


```AddEnableRouteToSBList()```

④ 이동가능한 루트에 대한 정보를 SBList에 넣습니다.


```DisplaySystem.DisplayOut()```

게임 매니저에서 화면에 출력합니다.


```ActByInput(int i) 혹은 ChageLocation(LocationType type)```

⑤ 선택지에 따라서 입력을 나누어 반응을 합니다.

(입력을 나누는 것은 GameManager에서 하고 있습니다.)

여기서 주의해야 하는 점은 던전에서는 별도의 입력처리를 하고 있습니다.
 결국에는 한곳에서 입력처리를 전부 하지 못하는 상황이니 만큼 던전과 맵의 상호작용에 대한 구조가 아쉽습니다.

 <br>

### << **StoreSystem** >>

StoreSystem.cs 파일에 있으며, 인벤토리를 가진 상인에 대한 기능을 합니다.

아이템들을 한번에 인벤토리에 넣고, 플래이어가 선택한 아이템이 무엇인지 참조를 하게 해줍니다.

다시 한번 dynamic이 다양한 버그들의 원인이었습니다. 딱 맞는 상황이 아니고서야 dynamic은 지양해야 된다고 생각하게 되었습니다.

<br>

### << **GameManager** >>

GameManager.cs 파일에 있으며, 게임에 대한 정보를 세팅하고 메인 루프를 돌리는 기능을 합니다.

<br>

### << **기타** >>

Program.cs 파일에 작성된 내용에 대해 설명하겠습니다.

```public static string StringWithCustomColor(string cout, int foreColor = 7, int backColor = 0)```
을 동작하기 위한 밑작업들로

```
[DllImport("kernel32.dll", SetLastError = true)]
public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
```

등등

이루어졌습니다.

```StringWithCustomColor```

이 함수는 콘솔에서 다양한 색을 출력하기 위한 것으로, <https://stackoverflow.com/questions/7937256/custom-text-color-in-c-sharp-console-application> 해당 링크에 대한 내용을 참고하였습니다.