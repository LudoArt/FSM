using UnityEngine;

namespace BaseFSM
{
    public class VisitBankAndDepositGold : State<Miner>
    {
        private static VisitBankAndDepositGold m_instance;
        public static VisitBankAndDepositGold GetInstance()
        {
            return m_instance;
        }

        public VisitBankAndDepositGold()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(Miner miner)
        {
            if (miner.Location() != LOCATION_TYPE.BANK)
            {
                Debug.Log(miner.GetNameOfEntity() + ": 去银行了。");
                miner.ChangeLocation(LOCATION_TYPE.BANK);
            }
        }

        public override void Execute(Miner miner)
        {
            miner.AddToWealth(miner.GoldCarried());

            miner.SetGoldCarried(0);
            Debug.Log(miner.GetNameOfEntity() + ": 现在有这么多黄金了: " + miner.Wealth());

            if (miner.Wealth() >= Miner.ComfortLevel)
            {
                Debug.Log(miner.GetNameOfEntity() + ": 非常富裕了，决定回家摸鱼。");
                miner.GetFSM().ChangeState(GoHomeAndSleepTilRested.GetInstance());
            }
            else
            {
                miner.GetFSM().ChangeState(EnterMineAndDigForNugget.GetInstance());
            }
        }

        public override void Exit(Miner miner)
        {
            Debug.Log(miner.GetNameOfEntity() + ": 离开了银行。");
        }
    }
}