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
            mInput = 0;
        }

        public int EnterDisplay()
        {
            Console.WriteLine("던전에 진입 중입니다.");
            Console.WriteLine("난이도를 선택해주세요.");
            // 0 이지, 1 노말, 2 하드
            Console.WriteLine();
            Console.WriteLine("[0] Easy");
            Console.WriteLine("[1] Normal");
            Console.WriteLine("[2] Hard");
            Console.WriteLine();
            return 3;
        }
        public int FightDisplay()
        {
            if (mInput == 0)
            {
                Console.WriteLine("전투 진행 중입니다.");
            }
            else if (mInput == 1)
            {

            }
            else if (mInput == 2)
            {
                mPlayer.Display();
            }
            // 0 진행, 1 탈출, 2 상태창 보기
            Console.WriteLine();
            Console.WriteLine("[0] 진행");
            Console.WriteLine("[1] 탈출");
            Console.WriteLine("[2] 상태창 보기");
            Console.WriteLine();
            return 3;
        }
        public int RestDisplay()
        {
            if (mInput == 0)
            {
                Console.WriteLine("휴식 중입니다.");
            }
            else if (mInput == 1)
            {

            }
            else if (mInput == 2)
            {
                mPlayer.Display();
            }
            // 0 진행, 1 탈출, 2 상태창 보기
            Console.WriteLine();
            Console.WriteLine("[0] 진행");
            Console.WriteLine("[1] 탈출");
            Console.WriteLine("[2] 상태창 보기");
            Console.WriteLine();
            return 3;
        }
        public int ExitDisplay()
        {
            if (mInput == 0)
            {
                Console.WriteLine("던전 탐험 결과입니다.");
                // 결과 표시
            }
            else if (mInput == 1)
            {
                mPlayer.Display();
            }
            // 0 확인, 1 상태창 보기
            Console.WriteLine();
            Console.WriteLine("[0] 확인");
            Console.WriteLine("[1] 상태창 보기");
            Console.WriteLine();
            return 2;
        }
        public bool ChangeState(int input)
        {
            bool result = true;
            mInput = input;
            switch (State)
            {
                case DungeonState.Enter:
                    // 0 이지, 1 노말, 2 하드
                    if (input < 3)
                    {
                        // 스테이지 초기화
                        mStage = new Stage(input);
                        State = mStage.OnGoing();
                    }
                    mInput = 0;
                    break;
                case DungeonState.Fight:
                    // 0 진행, 1 탈출, 2 상태창 보기
                    if (input == 0)
                    {
                        if (mStage != null)
                            State = mStage.OnGoing();
                    }
                    else if (input == 1)
                    {
                        State = DungeonState.Exit;
                        if (mStage != null)
                            mStage.ExitStage();
                        mInput = 0;
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
                        if (mStage != null)
                            State = mStage.OnGoing();
                    }
                    else if (input == 1)
                    {
                        State = DungeonState.Exit;
                        if (mStage != null)
                            mStage.ExitStage();
                        mInput = 0;
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
                        result = false;
                    }
                    else if (input == 1)
                    {
                        // 상태 출력
                    }
                    break;
                default:
                    break;
            }
            return result;
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

        

        public class Stage
        {
            public int Level { get; private set; }
            private Random mRand;

            public Stage(int level) 
            {
                Level = level;
                mRand = new Random();
            }

            public DungeonState OnGoing()
            {
                DungeonState result = DungeonState.Fight;
                // 난이도 및 진행에 따른 확률에 의해 Fight/Rest 변경
                return result;
            }

            public void ExitStage()
            {

            }
        }
    }
}
