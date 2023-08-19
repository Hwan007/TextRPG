﻿using System.Text.Json;
using static TextRPG;

public class JsonFileIOStream
{
    JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };

    private string PathFromDesktop()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        return desktopPath;
    }

    private string PathFromCurrent()
    {
        string? currentPath = Environment.ProcessPath.Split("TextRPGGame")[0] + "TextRPGGame\\Data";
        return currentPath ?? PathFromDesktop();
    }

    public T? LoadFile<T>(string fileName)
    {
        string jsonString = File.ReadAllText(Path.Combine(PathFromCurrent(), fileName));
        var item = JsonSerializer.Deserialize<T>(jsonString);
        return item;
    }

    public void SaveFile<T>(string fileName, T items)
    {
        string jsonString = JsonSerializer.Serialize(items, jsonOptions);
        string path = PathFromCurrent();
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, fileName), jsonString);
    }

    public bool CheckFile(string fileName)
    {
        return File.Exists(PathFromCurrent());
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
            new Weapon("가검", 10, "날이 뭉툭해서 뭉둥이와 다를바 없습니다."),
            new Weapon("야구 방망이", 15, "때리는 손맛이 좋습니다."),
            new Weapon("소방 도끼", 25, "빨간색이 인상적인 도끼입니다."),
            new Weapon("스파타", 30, "로마 제국에서 사용하던 검입니다."),
            new Weapon("꽝꽝정어리", 50, "무기로 써도 될 만큼 꽝꽝 얼었습니다."),
            new Weapon("생생정어리", 100, "강력하게 펄떡이는 정어리입니다.")
        };
        SaveFile<Weapon[]>(mWeaponFileName, weapons);

        Armor[] armors = new Armor[]
        {
            new Armor("천 갑옷",10,"손때가 뭍어있는 천으로 이루어진 갑옷입니다."),
            new Armor("가죽 갑옷",15,"약간 쿰쿰한 냄새가 나는 가죽 갑옷입니다."),
            new Armor("체인 메일",25,"왠만한 무기는 이를 뚫을 수 없을 겁니다."),
            new Armor("풀 플레이트",30,"무한한 자신감을 불어넣어 주는 풀 플래이트입니다."),
            new Armor("프라이팬",100,"당신의 엉덩이를 지켜주는 소중한 친구입니다.")
        };
        SaveFile<Armor[]>(mArmorFileName, armors);
    }
}