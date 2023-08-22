using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using static TextRPG;

public class JsonFileIOStream
{
    public static JsonSerializerOptions JsonOptions = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };

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
        string jsonString = JsonSerializer.Serialize(items, JsonOptions);
        string path = PathFromCurrent();
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, fileName), jsonString);
    }

    public static bool CheckFile(string fileName)
    {
        return File.Exists(Path.Combine(PathFromCurrent(), fileName));
    }

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
}

internal partial class TextRPG
{
    public class CharacterConverter : JsonConverter<CharacterSystem>
    {
        public override CharacterSystem? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            JsonNode? jsonNode = JsonObject.Parse(ref reader);
            JsonNode? jsonNode1 = JsonObject.Parse(ref reader);
            JsonNode? jsonNode2 = JsonObject.Parse(ref reader);
            //Character


            throw new NotImplementedException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            CharacterSystem value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            {
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
                // Exp
                writer.WritePropertyName(nameof(value.Exp));
                writer.WriteNumberValue(value.Exp);
            }
            
            // Inven
            writer.WriteStartObject(nameof(value.Inven));
            {
                // mItems
                //writer.WriteStartArray(nameof(value.Inven.mItems));
                //{
                //    // Data
                //    var itemData = value.Inven.mItems.First;
                //    for (int i = 0; i < value.Inven.mItems.Count; ++i)
                //    {
                //        if (itemData == null)
                //            break;
                //        writer.WriteStartObject(nameof(itemData.ValueRef.Data));
                //        {
                //            // Name
                //            writer.WritePropertyName(nameof(itemData.ValueRef.Data.Name));
                //            writer.WriteStringValue(itemData.ValueRef.Data.Name);
                //            // Description
                //            writer.WritePropertyName(nameof(itemData.ValueRef.Data.Description));
                //            writer.WriteStringValue(itemData.ValueRef.Data.Description);
                //            // IsEquip
                //            writer.WritePropertyName(nameof(itemData.ValueRef.Data.IsEquip));
                //            writer.WriteBooleanValue(itemData.ValueRef.Data.IsEquip);
                //            // Point
                //            writer.WritePropertyName(nameof(itemData.ValueRef.Data.Point));
                //            writer.WriteNumberValue(itemData.ValueRef.Data.Point);
                //            // Gold
                //            writer.WritePropertyName(nameof(itemData.ValueRef.Data.Gold));
                //            writer.WriteNumberValue(itemData.ValueRef.Data.Gold);
                //        }
                //        writer.WriteEndObject();
                //        itemData = itemData?.Next;
                //    }
                //}
                //writer.WriteEndArray();
            }
            // Equipments
            writer.WriteStartObject(nameof(value.Equipments));
            {
                writer.WriteStartArray(nameof(value.Equipments.EquipItemList));
                // EquipItemList
                {
                    // ItemRef
                    //var item = value.Equipments.EquipItemList.First;
                    //for (int i = 0; i < value.Equipments.EquipItemList.Count; ++i)
                    //{
                    //    if (item == null)
                    //        break;
                    //    writer.WriteStartObject(nameof(item.ValueRef.ItemRef));
                    //    // Data
                    //    writer.WriteStartObject(nameof(item.ValueRef.ItemRef.Data));
                    //    {
                    //        // type
                    //        writer.WritePropertyName(item.ValueRef.ItemRef.Data.type);
                    //        writer.WriteNumberValue((int)item.ValueRef.ItemRef.Data.type);
                    //        // Name
                    //        writer.WritePropertyName(item.ValueRef.ItemRef.Data.Name);
                    //        writer.WriteStringValue(item.ValueRef.ItemRef.Data.Name);
                    //        // Description
                    //        writer.WritePropertyName(item.ValueRef.ItemRef.Data.Description);
                    //        writer.WriteStringValue(item.ValueRef.ItemRef.Data.Description);
                    //        // IsEquip
                    //        writer.WritePropertyName(item.ValueRef.ItemRef.Data.IsEquip);
                    //        writer.WriteBooleanValue(item.ValueRef.ItemRef.Data.IsEquip);
                    //        // Point
                    //        writer.WritePropertyName(item.ValueRef.ItemRef.Data.Point);
                    //        writer.WriteNumberValue(item.ValueRef.ItemRef.Data.Point);
                    //        // Gold
                    //        writer.WritePropertyName(item.ValueRef.ItemRef.Data.Gold);
                    //        writer.WriteNumberValue(item.ValueRef.ItemRef.Data.Gold);
                    //    }
                    //    writer.WriteEndObject();
                    //    writer.WriteEndObject();
                    //    item = item.Next;
                    //}
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
            
        }
    }
}