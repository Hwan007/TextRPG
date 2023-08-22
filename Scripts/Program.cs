using System.Runtime.InteropServices;

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
