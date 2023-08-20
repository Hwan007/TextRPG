internal partial class TextRPG
{
    public class Dungeon : IDisplay
    {
        public DungeonState State { get; private set; }
        public enum DungeonState
        {
            Enter,
            Fight,
            Rest,
            Exit
        }

        public void EnterDisplay()
        {

        }
        public void FightDisplay()
        {

        }
        public void RestDisplay()
        {

        }
        public void ExitDisplay()
        {

        }
        public void ChangeState(int input)
        {
            switch (State)
            {
                case DungeonState.Enter:
                    // 0 진입, 1 상태창 보기
                    break;
                case DungeonState.Fight:
                    // 0 진행, 1 상태창 보기
                    break;
                case DungeonState.Rest:
                    // 0 진행, 1 상태창 보기
                    break;
                case DungeonState.Exit:
                    // 0 진행, 1 상태창 보기
                    break;
            }
        }

        public int Display()
        {
            return 2;
        }
    }
}
