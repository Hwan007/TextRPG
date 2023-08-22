using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using static TextRPG;

public class JsonFileIOStream
{
    static JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };

    private static string PathFromDesktop()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        return desktopPath;
    }

    private static string PathFromCurrent()
    {
        string? currentPath = Environment.ProcessPath?.Split("TextRPGGame")[0] + "TextRPGGame\\Data";
        return currentPath ?? PathFromDesktop();
    }

    public static T? LoadFile<T>(string fileName)
    {
        string jsonString = File.ReadAllText(Path.Combine(PathFromCurrent(), fileName));
        var item = JsonSerializer.Deserialize<T>(jsonString);
        return item;
    }

    public static void SaveFile<T>(string fileName, T items)
    {
        string jsonString = JsonSerializer.Serialize(items, jsonOptions);
        string path = PathFromCurrent();
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, fileName), jsonString);
    }

    public static bool CheckFile(string fileName)
    {
        return File.Exists(Path.Combine(PathFromCurrent(), fileName));
    }




    //string testFileName = "Item.json";
    //public void TestSave()
    //{
    //    Weapon[] item = new Weapon[] {
    //        new Weapon("초보자 검", 10, "날이 뭉툭해서 뭉둥이와 다를바 없습니다."),
    //        new Weapon("몰락한 왕의 검", 100, "한때 영광스러운 왕국의 왕이 사용했던 검입니다."),
    //        new Weapon("꽝꽝정어리", 15, "무기로 써도 될 만큼 꽝꽝 얼었습니다.")
    //    };
    //    //string jsonString = JsonSerializer.Serialize(item, jsonOptions);
    //    //Console.WriteLine(jsonString);

    //    //File.WriteAllText(PathFromDesktop(testFileName), jsonString);

    //    SaveFile<Weapon[]>(testFileName, item);
    //}

    //public void TestLoad()
    //{
    //    //string jsonString = File.ReadAllText(PathFromDesktop(testFileName));
    //    //var item = JsonSerializer.Deserialize<Weapon[]>(jsonString);

    //    //item.Display();
    //    //Console.WriteLine();
    //    //Console.WriteLine(item.Data.type);
    //    var items = LoadFile<Weapon[]>(testFileName);

    //    if (items is Weapon[])
    //    {
    //        foreach (var item in items)
    //        {
    //            item.Display();
    //            Console.WriteLine();
    //        }
    //    }
    //}

    string mWeaponFileName = "Weapon.json";
    string mArmorFileName = "Armor.json";
    public void SaveItemDataBase()
    {
        Weapon[] weapons = new Weapon[]
        {
            new Weapon("가검", 10, "날이 뭉툭해서 뭉둥이와 다를바 없습니다.", 100),
            new Weapon("야구방망이", 15, "때리는 손맛이 좋습니다.", 200),
            new Weapon("소방도끼", 25, "빨간색이 인상적인 도끼입니다.", 450),
            new Weapon("스파타", 30, "로마 제국에서 사용하던 검입니다.", 600),
            new Weapon("꽝꽝정어리", 50, "무기로 써도 될 만큼 꽝꽝 얼었습니다.", 1000),
            new Weapon("생생정어리", 100, "싱싱한 정어리입니다.", 2000)
        };
        SaveFile<Weapon[]>(mWeaponFileName, weapons);

        Armor[] armors = new Armor[]
        {
            new Armor("천갑옷",10,"손때가 뭍어있는 천으로 이루어진 갑옷입니다.", 100),
            new Armor("가죽갑옷",15,"약간 쿰쿰한 냄새가 나는 가죽 갑옷입니다.", 200),
            new Armor("체인메일",25,"왠만한 무기는 이를 뚫을 수 없을 겁니다.", 450),
            new Armor("풀플레이트",30,"자신감을 불어넣어 주는 갑옷입니다.", 600),
            new Armor("프라이팬",100,"총알도 뚫을 수 없는 프라이팬입니다.", 1000)
        };
        SaveFile<Armor[]>(mArmorFileName, armors);
    }

    private class CharacterConverter : JsonConverter<CharacterSystem>
    {
        public override CharacterSystem? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            JsonNode? jn = JsonObject.Parse(ref reader);
            string? name = jn["Name"].ToString();

            throw new NotImplementedException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            CharacterSystem value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            // Name
            writer.WritePropertyName(nameof(value.Name));
            writer.WriteStringValue(value.Name);
            // Job
            writer.WritePropertyName(nameof(value.Job));
            writer.WriteStringValue(value.Job);
            // Level
            writer.WritePropertyName(nameof(value.Level));
            writer.WriteNumberValue(value.Level);
            // baseAtk
            writer.WritePropertyName(nameof(value.baseAtk));
            writer.WriteNumberValue(value.baseAtk);
            // baseDef
            writer.WritePropertyName(nameof(value.baseDef));
            writer.WriteNumberValue(value.baseDef);
            // Hp
            writer.WritePropertyName(nameof(value.Hp));
            writer.WriteNumberValue(value.Hp);
            // baseHP
            writer.WritePropertyName(nameof(value.baseHP));
            writer.WriteNumberValue(value.baseHP);
            // Gold
            writer.WritePropertyName(nameof(value.Level));
            writer.WriteNumberValue(value.Level);
            // Inven
            writer.WritePropertyName(nameof(value.Inven));
            writer.WriteStartObject();
            {
                // mItems
                writer.WritePropertyName(nameof(value.Inven.mItems));
                writer.WriteStartObject();
                {
                    // Data
                    var itemData = value.Inven.mItems.First;
                    for (int i = 0; i < value.Inven.mItems.Count; ++i)
                    {
                        if (itemData == null)
                            break;
                        writer.WritePropertyName(nameof(itemData.ValueRef.Data));
                        writer.WriteStartObject();
                        // type
                        writer.WritePropertyName(nameof(itemData.ValueRef.Data));
                        // Name
                        // Description
                        // IsEquip
                        // Point
                        // Gold
                        writer.WriteEndObject();
                        itemData = itemData?.Next;
                    }
                }
                writer.WriteEndObject();
            }
            // Equipments
            {
                // EquipItemList
                {
                    // ItemRef
                    {
                        // ATK
                        // Data
                        // type
                        // Name
                        // Description
                        // IsEquip
                        // Point
                        // Gold
                    }
                }
            }
            // Exp
            writer.WriteEndObject();
        }

    }
}