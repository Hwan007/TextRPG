using System.Runtime.InteropServices;
using System.Text;

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

        //JsonFileIOStream JsonIO = new JsonFileIOStream();
        //JsonIO.SaveItemDataBase();

        TextRPG.GameManager game = new TextRPG.GameManager();
        game.GameStart();
    }
}

internal partial class TextRPG
{
    /// <summary>
    /// 화면에 뿌리는 부모 함수
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

    public static void WriteWithColor(string cout, ConsoleColor charColor, ConsoleColor backColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = charColor;
        Console.BackgroundColor = backColor;
        Console.Write(cout);
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
    }

    public static void WriteWithCustomColor(string cout, int foreColor = 7, int backColor = 0)
    {
        Console.Write("\x1b[38;5;" + foreColor % 255 + "m\x1b[48;5;" + backColor % 255 + $"m{cout}");
        Console.Write("\x1b[38;5;15m\x1b[48;5;0;m");
    }

    public static void WriteWithCustomColor(string cout, ColorType foreColor, int backColor = 0)
    {
        Console.Write("\x1b[38;5;" + (int)foreColor % 255 + "m\x1b[48;5;" + backColor % 255 + $"m{cout}");
        Console.Write("\x1b[38;5;15m\x1b[48;5;0;m");
    }

    public static string StringWithCustomColor(string cout, int foreColor = 7, int backColor = 0)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("\x1b[38;5;" + foreColor % 255 + "m\x1b[48;5;" + backColor % 255 + $"m{cout}");
        sb.Append("\x1b[38;5;15m\x1b[48;5;0;m");
        return sb.ToString();
    }

    public static string StringWithCustomColor(string cout, ColorType foreColor, int backColor = 0)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("\x1b[38;5;" + (int)foreColor % 255 + "m\x1b[48;5;" + backColor % 255 + $"m{cout}");
        sb.Append("\x1b[38;5;15m\x1b[48;5;0;m");
        return sb.ToString();
    }

    public enum ColorType
    {
        Gold = 178,
        Red = 160,
        Gray = 7,
        White = 15,
        Black = 16,
        Orange = 208,
        PurpleBlue = 75
    }
}
