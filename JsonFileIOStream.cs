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

    public T[]? LoadFile<T>(string fileName)
    {
        string jsonString = File.ReadAllText(PathFromDesktop(fileName));
        var item = JsonSerializer.Deserialize<T[]>(jsonString);
        return item;
    }

    public void SaveFile<T>(string fileName, T[] items)
    {
        string jsonString = JsonSerializer.Serialize(items, jsonOptions);
        File.WriteAllText(PathFromDesktop(fileName), jsonString);
    }

    string testFileName = "Item.json";
    public void TestSave()
    {
        Weapon item = new Weapon("초보자 검", 10, "날이 뭉툭해서 뭉둥이와 다를바 없습니다.");
        string jsonString = JsonSerializer.Serialize(item, jsonOptions);
        Console.WriteLine(jsonString);

        File.WriteAllText(PathFromDesktop(testFileName), jsonString);
    }

    public void TestLoad()
    {
        string jsonString = File.ReadAllText(PathFromDesktop(testFileName));
        var item = JsonSerializer.Deserialize<Weapon>(jsonString);

        item.Display();
        Console.WriteLine();
        Console.WriteLine(item.Data.type);
    }
}