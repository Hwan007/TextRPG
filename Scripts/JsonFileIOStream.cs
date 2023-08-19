using System.Text.Json;
using static TextRPG;

public class JsonFileIOStream
{
    JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };

    private string PathFromDesktop(string fileName)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        return Path.Combine(desktopPath, fileName);
    }

    private string PathFromCurrent(string fileName)
    {
        string? currentPath = Environment.ProcessPath;
        return currentPath ?? PathFromDesktop(fileName);
    }

    public T? LoadFile<T>(string fileName)
    {
        string jsonString = File.ReadAllText(PathFromCurrent(fileName));
        var item = JsonSerializer.Deserialize<T>(jsonString);
        return item;
    }

    public void SaveFile<T>(string fileName, T items)
    {
        string jsonString = JsonSerializer.Serialize(items, jsonOptions);
        File.WriteAllText(PathFromCurrent(fileName), jsonString);
    }

    public bool CheckFile(string fileName)
    {
        return File.Exists(PathFromCurrent(fileName));
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
}