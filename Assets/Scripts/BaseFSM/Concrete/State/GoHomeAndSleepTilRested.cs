using UnityEngine;

namespace BaseFSM
{
    public class GoHomeAndSleepTilRested : State<Miner>
    {
        private static GoHomeAndSleepTilRested m_instance;
        public static GoHomeAndSleepTilRested GetInstance()
        {
            return m_instance;
        }

        public GoHomeAndSleepTilRested()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(Miner miner)
        {
            if (miner.Location() != LOCATION_TYPE.HOME)
            {
                Debug.Log(miner.GetNameOfEntity() + ": 回家了。");
                miner.ChangeLocation(LOCATION_TYPE.HOME);
            }
        }

        public override void Execute(Miner miner)
        {
            if (!miner.Fatigued())
            {
                Debug.Log(miner.GetNameOfEntity() + ": 醒了，发觉是时候开启打工人新的一天了。");
                miner.GetFSM().ChangeState(EnterMineAndDigForNugget.GetInstance());
            }
            else
            {
                miner.DecreaseFatigue();
                Debug.Log(miner.GetNameOfEntity() + ": 睡着了，发出了猪一样的呼噜声。");
            }
        }

        public override void Exit(Miner miner)
        {
            Debug.Log(miner.GetNameOfEntity() + ": 离开了他的猪圈。");
        }
    }
}