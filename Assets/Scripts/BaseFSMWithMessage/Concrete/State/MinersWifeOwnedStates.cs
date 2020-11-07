using UnityEngine;

namespace FSMMSG
{
    public class WifesGlobalState : State<MinersWife>
    {
        private static WifesGlobalState m_instance;
        public static WifesGlobalState GetInstance()
        {
            return m_instance;
        }

        public WifesGlobalState()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(MinersWife wife)
        {
        }

        public override void Execute(MinersWife wife)
        {
            // 有十分之一的几率去厕所
            if ((Random.Range(0.0f, 1.0f) < 0.1) && !wife.GetFSM().IsInState(VisitBathroom.GetInstance()))
            {
                wife.GetFSM().ChangeState(VisitBathroom.GetInstance());
            }
        }

        public override void Exit(MinersWife wife)
        {
        }

        public override bool OnMessage(MinersWife wife, Telegram msg)
        {
            switch (msg.Msg)
            {
                case message_type.Msg_HiHoneyImHome:
                    {
                        Debug.Log("Message handled by " + wife.GetNameOfEntity() + " at time: " + Clock.GetCurrentTime());
                        Debug.Log(wife.GetNameOfEntity() + ": 徐子杨回来了，大傻逼属性发动。");

                        wife.GetFSM().ChangeState(CookStew.GetInstance());
                    }
                    return true;

            }

            return false;
        }
    }

    public class DoHouseWork : State<MinersWife>
    {
        private static DoHouseWork m_instance;
        public static DoHouseWork GetInstance()
        {
            return m_instance;
        }

        public DoHouseWork()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(MinersWife wife)
        {
            Debug.Log(wife.GetNameOfEntity() + ": 是时候开始收拾屋子了。");
        }

        public override void Execute(MinersWife wife)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    Debug.Log(wife.GetNameOfEntity() + ": 擦地板。");
                    break;
                case 1:
                    Debug.Log(wife.GetNameOfEntity() + ": 洗盘子。");
                    break;
                case 2:
                    Debug.Log(wife.GetNameOfEntity() + ": 暖床？！");
                    break;
            }
        }

        public override void Exit(MinersWife wife)
        {
        }

        public override bool OnMessage(MinersWife wife, Telegram msg)
        {
            return false;
        }
    }

    public class VisitBathroom : State<MinersWife>
    {
        private static VisitBathroom m_instance;
        public static VisitBathroom GetInstance()
        {
            return m_instance;
        }

        public VisitBathroom()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(MinersWife wife)
        {
            Debug.Log(wife.GetNameOfEntity() + ": 去上厕所了。");
        }

        public override void Execute(MinersWife wife)
        {
            Debug.Log(wife.GetNameOfEntity() + ": 拉屎好爽。");
            wife.GetFSM().RevertToPreviousState();
        }

        public override void Exit(MinersWife wife)
        {
            Debug.Log(wife.GetNameOfEntity() + ":菊花又出血了。");
        }

        public override bool OnMessage(MinersWife wife, Telegram msg)
        {
            return false;
        }
    }

    public class CookStew : State<MinersWife>
    {
        private static CookStew m_instance;
        public static CookStew GetInstance()
        {
            return m_instance;
        }

        public CookStew()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        public override void Enter(MinersWife wife)
        {
            if (!wife.Cooking())
            {
                Debug.Log(wife.GetNameOfEntity() + ": 开始做饭了。");

                // 给自己发一个延迟消息
                MessageDispatcher.GetInstance().DispatchMessage(1.5f, wife.ID(), wife.ID(), message_type.Msg_StewReady);

                wife.SetCooking(true);
            }

        }

        public override void Execute(MinersWife wife)
        {
            Debug.Log(wife.GetNameOfEntity() + ": 做饭中...");
        }

        public override void Exit(MinersWife wife)
        {
            Debug.Log(wife.GetNameOfEntity() + ": 把做完的黑暗料理摆桌上。");
        }

        public override bool OnMessage(MinersWife wife, Telegram msg)
        {
            switch (msg.Msg)
            {
                case message_type.Msg_StewReady:
                    Debug.Log("Message received by " + wife.GetNameOfEntity() + " at time: " + Clock.GetCurrentTime());
                    Debug.Log(wife.GetNameOfEntity() + ": 饭好了！快来恰饭！");

                    //let hubby know the stew is ready
                    MessageDispatcher.GetInstance().DispatchMessage(0, wife.ID(), 0, message_type.Msg_StewReady);

                    wife.SetCooking(false);

                    wife.GetFSM().ChangeState(DoHouseWork.GetInstance());
                    return true;
            }
            return false;
        }
    }
}