internal partial class TextRPG
{
    public class DungeonSystem : DisplaySystem
    {
        public DungeonState State { get; private set; }
        private int mInput;
        private Stage? mStage;
        private CharacterSystem mPlayer;
        public enum DungeonState
        {
            Enter,
            Fight,
            Rest,
            Exit
        }
        public DungeonSystem(CharacterSystem player)
        {
            State = DungeonState.Enter;
            mPlayer = player;
            mInput = 0;
        }

        public int EnterDisplay()
        {
            AddStringToDisplayList("던전에 진입 중입니다.\n");
            AddStringToDisplayList("난이도를 선택해주세요.\n");
            // 0 이지, 1 노말, 2 하드
            AddStringToDisplayList("[0]\t [1]\t [2]\n");
            AddStringToDisplayList("Easy\tNormal\tHard\n");
            return 3;
        }
        public int FightDisplay()
        {
            // 0 진행, 1 탈출, 2 상태창 보기

            if (mInput == 0)
            {
                AddStringToDisplayList($"{mStage?.Level}\n전투 진행 중입니다.\n");
                if (mStage != null)
                {
                    if (mPlayer.IsDead)
                    {
                        AddStringToDisplayList(StringWithCustomColor($"{mStage.Result.Damage}", 160));
                        AddStringToDisplayList(StringWithCustomColor(" 대미지를 입었습니다.\n"));
                        AddStringToDisplayList(StringWithCustomColor($"플래이어의 HP가 0이 되어 "));
                        AddStringToDisplayList(StringWithCustomColor("사망", 160));
                        AddStringToDisplayList(StringWithCustomColor("하였습니다.\n"));
                    }
                    else if(mStage.Result.Damage != 0)
                    {
                        AddStringToDisplayList(StringWithCustomColor($"{mStage.Result.Damage}", 160));
                        AddStringToDisplayList(StringWithCustomColor(" 대미지를 입었습니다.\n"));
                        AddStringToDisplayList(StringWithCustomColor($"{mStage.Result.Gold} G", 178));
                        AddStringToDisplayList(StringWithCustomColor(" 를 얻었습니다.\n"));
                        AddStringToDisplayList(StringWithCustomColor($"지금까지 총 "));
                        AddStringToDisplayList(StringWithCustomColor($"{mStage.TotalGold} G", 178));
                        AddStringToDisplayList(StringWithCustomColor(" 를 얻었습니다.\n"));
                    }
                }
            }
            else if (mInput == 1)
            {

            }
            else if (mInput == 2)
            {
                mPlayer.SetDisplayString();
            }
            
            AddStringToDisplayList("\n");
            if (mPlayer.IsDead)
            {
                AddStringToDisplayList("[0] 탈출");
            }
            else
            {
                AddStringToDisplayList("[0] 진행");
                AddStringToDisplayList("[1] 탈출");
                AddStringToDisplayList("[2] 상태창 보기");
            }
            AddStringToDisplayList("\n");
            return 3;
        }
        public int RestDisplay()
        {
            AddStringToDisplayList("휴식 중입니다.");
            if (mStage != null)
            {
                if (mStage.Result.Damage != 0)
                {
                    AddStringToDisplayList(StringWithCustomColor($"{mStage.Result.Heal}", 34));
                    AddStringToDisplayList(StringWithCustomColor(" 체력을 회복하였습니다.\n"));
                    AddStringToDisplayList(StringWithCustomColor($"지금까지 총 "));
                    AddStringToDisplayList(StringWithCustomColor($"{mStage.TotalGold} G", 178));
                    AddStringToDisplayList(StringWithCustomColor(" 를 얻었습니다.\n"));
                }
            }
            if (mInput == 0)
            {
                
            }
            else if (mInput == 1)
            {

            }
            else if (mInput == 2)
            {
                mPlayer.SetDisplayString();
            }
            // 0 진행, 1 탈출, 2 상태창 보기
            AddStringToDisplayList("\n[0] 진행\n");
            AddStringToDisplayList("[1] 탈출\n");
            AddStringToDisplayList("[2] 상태창 보기\n\n");
            return 3;
        }
        public int ExitDisplay()
        {
            AddStringToDisplayList("던전 탐험 결과입니다.\n");
            AddStringToDisplayList("지금까지 총 " + StringWithCustomColor($"{mStage?.TotalGold} G", 178) + " 를 얻었습니다.\n");
            // 결과 표시
            if (mInput == 0)
            {

            }
            else if (mInput == 1)
            {
                mPlayer.SetDisplayString();
            }
            // 0 확인, 1 상태창 보기
            AddStringToDisplayList("\n[0] 확인\n");
            AddStringToDisplayList("[1] 상태창 보기\n\n");
            return 2;
        }
        public bool ChangeState(int input)
        {
            bool result = true;
            mInput = input;
            if (mPlayer.IsDead)
            {
                return false;
            }
            switch (State)
            {
                case DungeonState.Enter:
                    // 0 이지, 1 노말, 2 하드
                    if (input < 3)
                    {
                        // 스테이지 초기화
                        mStage = new Stage(mPlayer, input);
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
                        if (mStage != null)
                            State = mStage.ExitStage();
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
                        if (mStage != null)
                            State = mStage.ExitStage();
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

        public override int SetDisplayString()
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
            // 0 이지, 1 노말, 2 하드
            public int Level { get; private set; }
            public int Gold { get; private set; }
            public int TotalGold { get; private set; }
            private Random mRand;
            private int[] mRecommendedDEF = new int[3] { 5, 15, 40 };
            private int[] mReward = new int[3] { 100, 170, 250 };
            private CharacterSystem mPlayer;
            public FightResult Result { get; private set; }

            public Stage(CharacterSystem player, int level)
            {
                Level = level;
                mRand = new Random();
                mPlayer = player;
                Result = new FightResult();
            }

            public DungeonState OnGoing()
            {
                DungeonState result = DungeonState.Fight;
                // 난이도 및 진행에 따른 확률에 의해 Fight/Rest 변경
                int fightOrRest = mRand.Next(10+Level*2);

                if (fightOrRest > 2)
                {
                    // 전투
                    int damage = Math.Clamp(mRand.Next(Convert.ToInt32(20f + (mRecommendedDEF[Level] - mPlayer.Def)), Convert.ToInt32(36f + (mRecommendedDEF[Level] - mPlayer.Def))), 1, 1000);
                    int gold = mRand.Next(Convert.ToInt32(mReward[Level] * (100f + mPlayer.Atk * 2f) / 200f), Convert.ToInt32(mReward[Level] * (100f + mPlayer.Atk * 2f) * 3f / 200f));
                    mPlayer.TakeDamage(damage);
                    Result.Gold = gold;
                    Result.Damage = damage;
                    TotalGold += gold;
                }
                else
                {
                    // 휴식
                    int heal = mRand.Next(1, mPlayer.Hp / 5);
                    mPlayer.TakeHeal(heal);
                    Result.Heal = heal;
                    result = DungeonState.Rest;
                }

                return result;
            }

            public DungeonState ExitStage()
            {
                DungeonState result = DungeonState.Exit;
                mPlayer.GetGold(TotalGold);
                return result;
            }

            public class FightResult
            {
                public int Gold { get; set; } = 0;
                public int Heal { get; set; } = 0;
                public int Damage { get; set; } = 0;
            }
        }
    }
}
