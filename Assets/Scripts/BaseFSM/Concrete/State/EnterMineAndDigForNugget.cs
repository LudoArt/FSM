using UnityEngine;

namespace BaseFSM
{
    public class EnterMineAndDigForNugget : State<Miner>
    {
        private static EnterMineAndDigForNugget m_instance;
        public static EnterMineAndDigForNugget GetInstance()
        {
            return m_instance;
        }

        public EnterMineAndDigForNugget()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(Miner miner)
        {
            if (miner.Location() != LOCATION_TYPE.GOLDMINE)
            {
                Debug.Log(miner.GetNameOfEntity() + ": 进入了金矿。");
                miner.ChangeLocation(LOCATION_TYPE.GOLDMINE);
            }
        }

        public override void Execute(Miner miner)
        {
            miner.AddToGoldCarried(1);
            miner.IncreaseFatigue();
            Debug.Log(miner.GetNameOfEntity() + ": 捡起了一个金块。");
            if (miner.PocketsFull())
                miner.GetFSM().ChangeState(VisitBankAndDepositGold.GetInstance());
            if (miner.Thirsty())
                miner.GetFSM().ChangeState(QuenchThirst.GetInstance());
        }

        public override void Exit(Miner miner)
        {
            Debug.Log(miner.GetNameOfEntity() + ": 离开了金矿。");
        }
    }
}