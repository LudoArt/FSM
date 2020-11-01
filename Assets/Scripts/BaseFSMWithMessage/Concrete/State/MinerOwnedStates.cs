using UnityEngine;

namespace FSMMSG
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
                MessageDispatcher.GetInstance().DispatchMessage(0, miner.ID(), 1, message_type.Msg_HiHoneyImHome);
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

        public override bool OnMessage(Miner miner, Telegram msg)
        {
            switch(msg.Msg)
            {
                case message_type.Msg_StewReady:
                    Debug.Log("Message received by " + miner.GetNameOfEntity() + " at time: " + Clock.GetCurrentTime());
                    Debug.Log(miner.GetNameOfEntity() + ": Okay Hun, ahm a comin'!");

                    miner.GetFSM().ChangeState(EatStew.GetInstance());
                    return true;
            }
            return false;
        }
    }

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

        public override bool OnMessage(Miner entity, Telegram msg)
        {
            return false;
        }
    }

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

        public override bool OnMessage(Miner entity, Telegram msg)
        {
            return false;
        }
    }

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

        public override bool OnMessage(Miner entity, Telegram msg)
        {
            return false;
        }
    }

    public class EatStew : State<Miner>
    {
        private static EatStew m_instance;
        public static EatStew GetInstance()
        {
            return m_instance;
        }

        public EatStew()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(Miner miner)
        {
            Debug.Log(miner.GetNameOfEntity() + ": Smells Reaaal goood Elsa!");
        }

        public override void Execute(Miner miner)
        {
            Debug.Log(miner.GetNameOfEntity() + ": Tastes real good too!");
            miner.GetFSM().RevertToPreviousState();
        }

        public override void Exit(Miner miner)
        {
            Debug.Log(miner.GetNameOfEntity() + ": Thankya li'lle lady. Ah better get back to whatever ah wuz doin'.");
        }

        public override bool OnMessage(Miner entity, Telegram msg)
        {
            return false;
        }
    }
}
