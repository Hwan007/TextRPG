internal partial class TextRPG
{
    public class Dungeon : IDisplay
    {
        public DungeonState State { get; private set; }
        private int mInput;
        private Stage? mStage;
        private Character mPlayer;
        public enum DungeonState
        {
            Enter,
            Fight,
            Rest,
            Exit
        }
        public Dungeon(Character player)
        {
            State = DungeonState.Enter;
            mPlayer = player;
        }

        public int EnterDisplay()
        {
            // 0 확인, 1 상태창 보기
            if (mInput == 0)
            {

            }
            else if (mInput == 1)
            {

            }
            else
            {

            }
            return 2;
        }
        public int FightDisplay()
        {
            // 0 진행, 1 탈출, 2 상태창 보기
            if (mInput == 0)
            {

            }
            else if (mInput == 1)
            {

            }
            else if (mInput == 2)
            {

            }
            else
            {

            }
            return 3;
        }
        public int RestDisplay()
        {
            // 0 진행, 1 탈출, 2 상태창 보기
            if (mInput == 0)
            {

            }
            else if (mInput == 1)
            {

            }
            else if (mInput == 2)
            {

            }
            else
            {

            }
            return 3;
        }
        public int ExitDisplay()
        {
            // 0 확인, 1 상태창 보기
            if (mInput == 0)
            {

            }
            else if (mInput == 1)
            {

            }
            else
            {

            }
            return 2;
        }
        public void ChangeState(int input)
        {
            mInput = input;
            switch (State)
            {
                case DungeonState.Enter:
                    // 0 이지, 1 노말, 2 하드
                    if (input < 3)
                    {
                        State = OnGoing();
                        // 스테이지 초기화
                        mStage = new Stage(input);
                    }
                    break;
                case DungeonState.Fight:
                    // 0 진행, 1 탈출, 2 상태창 보기
                    if (input == 0)
                    {
                        State = OnGoing();
                    }
                    else if (input == 1)
                    {
                        State = DungeonState.Exit;
                    }
                    else if (input == 2)
                    {
                        // 상태 출력
                    }
                    break;
                case DungeonState.Rest:
                    // 0 진행, 1 탈출, 2 상태창 보기
                    if (input == 0)
                    {
                        State = OnGoing();
                    }
                    else if (input == 1)
                    {
                        State = DungeonState.Exit;
                    }
                    else if (input == 2)
                    {
                        // 상태 출력
                    }
                    break;
                case DungeonState.Exit:
                    // 0 확인, 1 상태창 보기
                    if (input == 0)
                    {
                        State = DungeonState.Enter;
                        // Locate를 Dungeon으로 바꾸기
                    }
                    else if (input == 1)
                    {
                        // 상태 출력
                    }
                    break;
                default:
                    break;
            }
        }

        public int Display()
        {
            int choiceNumber;
            switch (State)
            {
                case DungeonState.Enter:
                    choiceNumber = EnterDisplay();
                    break;
                case DungeonState.Fight:
                    choiceNumber = FightDisplay();
                    break;
                case DungeonState.Rest:
                    choiceNumber = RestDisplay();
                    break;
                case DungeonState.Exit:
                    choiceNumber = ExitDisplay();
                    break;
                default:
                    choiceNumber = 0;
                    break;
            }
            return choiceNumber;
        }

        public DungeonState OnGoing()
        {
            DungeonState result = DungeonState.Fight;
            // 난이도 및 진행에 따른 확률에 의해 Fight/Rest 변경
            return result;
        }

        public class Stage
        {
            public int Level { get; private set; }

            public Stage(int level) 
            {
                Level = level;
            }
        }
    }
}
