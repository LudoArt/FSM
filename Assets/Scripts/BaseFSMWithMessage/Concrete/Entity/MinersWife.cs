
namespace FSMMSG
{
    public class MinersWife : BaseGameEntity
    {
        private StateMachine<MinersWife> m_StateMachine;
        private LOCATION_TYPE m_Location;
        private bool m_bCooking;


        public MinersWife(int ID, string name) : base(ID, name)
        {
            m_Location = LOCATION_TYPE.HOME;
            m_bCooking = false;

            m_StateMachine = new StateMachine<MinersWife>(this);
            m_StateMachine.SetCurrentState(DoHouseWork.GetInstance());
            m_StateMachine.SetGlobalState(WifesGlobalState.GetInstance());
        }

        public override void Update()
        {
            m_StateMachine.CustomUpdate();
        }

        public StateMachine<MinersWife> GetFSM()
        {
            return m_StateMachine;
        }

        public override bool HandleMessage(Telegram msg)
        {
            return m_StateMachine.HandleMessage(msg);
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

        public bool Cooking()
        {
            return m_bCooking;
        }
        public void SetCooking(bool val)
        {
            m_bCooking = val;
        }

        #endregion
    }
}


