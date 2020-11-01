
namespace BaseFSM
{
    public enum LOCATION_TYPE { GOLDMINE, BANK, BAR, HOME }

    public class Miner : BaseGameEntity
    {
        public const int ComfortLevel = 5;
        public const int MaxNuggets = 3;
        public const int ThirstLevel = 5;
        public const int TirednessThreshold = 5;

        /* 优化前代码
        /// <summary>
        /// 当前状态实例
        /// </summary>
        private State<Miner> m_CurrentState;
        /// <summary>
        /// 之前状态实m例
        /// </summary>
        private State<Miner> m_PreviousState;
        /// <summary>
        /// 全局状态实例
        /// </summary>
        private State<Miner> m_GlobalState;
        */

        /// <summary>
        /// state machine的一个实例（优化后）
        /// </summary>
        private StateMachine<Miner> m_StateMachine;


        #region 记录Miner的一些属性（与状态机核心内容无关）

        /// <summary>
        /// 矿工当前所处的位置
        /// </summary>
        private LOCATION_TYPE m_Location;
        /// <summary>
        /// 矿工包中装的天然金块
        /// </summary>
        private int m_iGoldCarried;
        /// <summary>
        /// 矿工在银行存的钱
        /// </summary>
        private int m_iMoneyInBank;
        /// <summary>
        /// 矿工的口渴程度
        /// </summary>
        private int m_iThirst;
        /// <summary>
        /// 矿工的劳累程度
        /// </summary>
        private int m_iFatigue;

        #endregion


        public Miner(int ID, string name) : base(ID, name)
        {
            m_Location = LOCATION_TYPE.HOME;
            m_iGoldCarried = 0;
            m_iMoneyInBank = 0;
            m_iThirst = 0;
            m_iFatigue = 0;

            m_StateMachine = new StateMachine<Miner>(this);
            m_StateMachine.SetCurrentState(GoHomeAndSleepTilRested.GetInstance());
            //m_StateMachine.SetGlobalState(MinerGlobalState.GetInstance());
        }

        public override void Update()
        {
            m_iThirst += 1;
            /* 优化前的代码
            if (m_CurrentState != null)
            {
                m_CurrentState.Execute(this);
            }
            */
            m_StateMachine.CustomUpdate(); // 优化后
        }

        /* 优化前的代码
        public void ChangeState(State<Miner> newState)
        {
            if (m_CurrentState == null || newState == null)
                return;

            m_PreviousState = m_CurrentState;
            m_CurrentState.Exit(this);
            m_CurrentState = newState;
            m_CurrentState.Enter(this);
        }

        public void RevertToPreviousState()
        {
            ChangeState(m_PreviousState);
        }
         */

        // 优化后
        public StateMachine<Miner> GetFSM()
        {
            return m_StateMachine;
        }

        #region 具体细节的工具类（与状态机核心内容无关）

        public LOCATION_TYPE Location()
        {
            return m_Location;
        }
        public void ChangeLocation(LOCATION_TYPE newLocation)
        {
            m_Location = newLocation;
        }


        public int GoldCarried()
        {
            return m_iGoldCarried;
        }
        public void SetGoldCarried(int val)
        {
            m_iGoldCarried = val;
        }
        public void AddToGoldCarried(int num)
        {
            m_iGoldCarried += num;
        }
        public bool PocketsFull()
        {
            if (m_iGoldCarried >= MaxNuggets)
                return true;
            else
                return false;
        }


        public bool Fatigued()
        {
            if (m_iFatigue > TirednessThreshold)
            {
                return true;
            }

            return false;
        }
        public void DecreaseFatigue()
        {
            m_iFatigue -= 1;
        }
        public void IncreaseFatigue()
        {
            m_iFatigue += 1;
        }


        public int Wealth()
        {
            return m_iMoneyInBank;
        }
        public void SetWealth(int val)
        {
            m_iMoneyInBank = val;
        }
        public void AddToWealth(int val)
        {
            m_iMoneyInBank += val;

            if (m_iMoneyInBank < 0) m_iMoneyInBank = 0;
        }


        public bool Thirsty()
        {
            if (m_iThirst >= ThirstLevel)
                return true;
            else
                return false;
        }
        public void BuyAndDrinkAWhiskey()
        {
            m_iThirst = 0;
            m_iMoneyInBank -= 2;
        }

        #endregion
    }
}