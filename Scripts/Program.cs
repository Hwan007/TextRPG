using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using static Program;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TextRPG;

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

        //JsonFileIOStream.jsonOptions.Converters.Add(new CharacterConverter());
        //JsonFileIOStream.SaveFile<CharacterSystem>("save1.json", new CharacterSystem("test", "ttest", 2, 2, 2, 2, 2));
        //JsonFileIOStream.LoadFile<CharacterSystem>("save1.json");
        List<InnerClass> innerClasses = new List<InnerClass>();
        innerClasses.Add(new InnerClass(new TestStruct() { str = "날아가라", num = 54 }));
        LinkedList<TestStruct> testStructs = new LinkedList<TestStruct>();
        testStructs.AddLast(new TestStruct() { str = "여친구함", num = 5959 });

        TestClass test = new TestClass(innerClasses, testStructs);
        TestConverter testConverter = new TestConverter();
        JsonFileIOStream.JsonOptions.Converters.Add(testConverter);

        JsonFileIOStream.SaveFile<TestClass>("test.json", test);
        var output = JsonFileIOStream.LoadFile<TestClass>("test.json");
        Console.WriteLine(output);

        /*
        for (int i = 0; i < 255; i++)
            Console.Write("\x1b[38;5;" + i + "m" + "\x1b[48;5;" + i + $"m■{(i > 15 ? ((i - 15) % (6) == 0 ? "\n" : "") : (i == 15) ? "\n" : "")}");
        Console.Write("\x1b[38;5;7m\x1b[48;5;0m");
        for (int i = 0; i < 255; i++)
            Console.Write("\x1b[48;5;" + i + $"m■{i}\t");
        Console.Write("\x1b[38;5;7m\x1b[48;5;0m");
        for (int i = 0; i < 255; i++)
            Console.Write("\x1b[38;5;" + i + $"m■{i}\t");
        Console.Write("\x1b[38;5;7m\x1b[48;5;0m");
        Console.ReadLine();

        for (int i = 0; i <= (int)ConsoleColor.White; ++i)
        {
            if (i < 6)
                Console.BackgroundColor = ConsoleColor.Gray;
            else
                Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = (ConsoleColor)i;
            Console.WriteLine("{0} {1} 글씨색깔을 미리보기 위한 테스트입니다.\n", i, (ConsoleColor)i);
        }
        for (int i = 0; i <= (int)ConsoleColor.White; ++i)
        {
            if (i < 7)
                Console.ForegroundColor = ConsoleColor.White;
            else
                Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = (ConsoleColor)i;
            Console.WriteLine("{0} {1} 배경색을 미리보기 위한 테스트입니다.\n", i, (ConsoleColor)i);
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
        */

        //JsonFileIOStream JsonIO = new JsonFileIOStream();
        //JsonIO.SaveItemDataBase();

        //GamaManager game = new GamaManager();
        //game.GameStart();
    }

    public class TestClass
    {
        public List<InnerClass> InnerClasses { get; private set; } = new List<InnerClass>();
        public LinkedList<TestStruct> TestStructs { get; private set; } = new LinkedList<TestStruct>();
        public int Num { get; private set; }

        [JsonConstructor]
        public TestClass(List<InnerClass> innerClasses, LinkedList<TestStruct> testStructs)
        {
            Num = 0;
            InnerClasses = innerClasses;
            TestStructs = testStructs;
        }
    }
    public class InnerClass
    {
        public TestStruct testStruct { get; private set; } = new TestStruct();
        [JsonConstructor]
        public InnerClass(TestStruct st)
        {
            testStruct = st;
        }
    }
    public struct TestStruct
    {
        public string str;
        public int num;
    }

    public class TestConverter : JsonConverter<TestClass>
    {
        public override TestClass? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int Num;
            List<InnerClass>? InnerClasses = new List<InnerClass>();
            LinkedList<TestStruct>? TestStructs = new LinkedList<TestStruct>();
            foreach (var (propertyName, value) in JsonNode.Parse(ref reader)!.AsObject())
            {
                if (value is null) continue;

                switch (propertyName)
                {
                    case "Num":
                        Num = value.GetValue<int>();
                        break;
                    case "InnerClasses":
                        InnerClasses = value.Deserialize<List<InnerClass>>();
                        break;
                    case "TestStructs":
                        TestStructs = value.Deserialize<LinkedList<TestStruct>>();
                        break;
                    default:
                        throw new InvalidOperationException($"unknown \"{propertyName}\".");
                }
            }
            return new TestClass(InnerClasses, TestStructs);
        }
        public override void Write(Utf8JsonWriter writer, TestClass value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteStartArray("InnerClasses");
            foreach (var node in value.InnerClasses)
            {
                JsonSerializer.Serialize(writer, node, options);
            }
            writer.WriteEndArray();
            writer.WriteStartArray("TestStructs");
            foreach (var node in value.TestStructs)
            {
                JsonSerializer.Serialize(writer, node, options);
            }
            writer.WriteEndArray();
            writer.WritePropertyName(nameof(value.Num));
            writer.WriteNumberValue(value.Num);
            writer.WriteEndObject();
        }
    }
}

internal partial class TextRPG
{
    /// <summary>
    /// 화면에 뿌리는 함수 표현 인터페이스
    /// </summary>
    public class DisplaySystem
    {
        public static List<string> SBList = new List<string>();
        public virtual int SetDisplayString()
        {
            return 0;
        }
        public virtual int SetDisplayString(bool show = false)
        {
            return 0;
        }
        public static int AddStringToDisplayList(string cout)
        {
            SBList.Add(cout);
            return SBList.Count;
        }
        public static void DisplayClear()
        {
            Console.Clear();
            SBList.Clear();
        }

        public static void DisplayOut()
        {
            foreach (string output in SBList)
            {
                Console.Write(output);
                Thread.Sleep(100);
            }
        }
    }
}
