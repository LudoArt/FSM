using UnityEngine;

namespace BaseFSM
{
    public class QuenchThirst : State<Miner>
    {
        private static QuenchThirst m_instance;
        public static QuenchThirst GetInstance()
        {
            return m_instance;
        }

        public QuenchThirst()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(Miner miner)
        {
            if (miner.Location() != LOCATION_TYPE.BAR)
            {
                miner.ChangeLocation(LOCATION_TYPE.BAR);
                Debug.Log(miner.GetNameOfEntity() + ": 渴了，去买肥宅快乐水喝。");
            }
        }

        public override void Execute(Miner miner)
        {
            if (miner.Thirsty())
            {
                miner.BuyAndDrinkAWhiskey();
                Debug.Log(miner.GetNameOfEntity() + ": 觉得肥宅快乐水真好喝。");
                miner.GetFSM().ChangeState(EnterMineAndDigForNugget.GetInstance());
            }
            else
            {
                Debug.Log("\nERROR!\nERROR!\nERROR!");
            }
        }

        public override void Exit(Miner miner)
        {
            Debug.Log(miner.GetNameOfEntity() + ": 喝完了很开心，离开了小卖部");
        }
    }
}