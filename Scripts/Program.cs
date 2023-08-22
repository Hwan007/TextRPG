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

        JsonFileIOStream JsonIO = new JsonFileIOStream();
        JsonIO.SaveItemDataBase();

        TextRPG.GamaManager game = new TextRPG.GamaManager();
        game.GameStart();
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
