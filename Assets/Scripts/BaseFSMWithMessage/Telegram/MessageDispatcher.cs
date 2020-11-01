using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMMSG
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum message_type
    {
        Msg_HiHoneyImHome,
        Msg_StewReady
    }

    /// <summary>
    /// 消息结构体
    /// </summary>
    public class Telegram
    {
        // 发送这个Telegram的实体
        public int Sender;
        // 接收这个Telegram的实体
        public int Receiver;
        // 信息本身
        public message_type Msg;
        //可以被立即发送或者延迟一个指定数量的时间后发送的消息
        // 如果一个延迟是必须的，这个域打上时间戳，消息应该在此时间后被发送
        public double DispatchTime;
        // 任何应该伴随消息的额外信息
        //void ExtraInfo; C++是通过空指针实现的，C#该如何实现呢？

        public Telegram(int senderID, int receiverID, message_type msg, double delayTime)
        {
            Sender = senderID;
            Receiver = receiverID;
            Msg = msg;
            DispatchTime = delayTime;
        }
        public int CompareTo(Telegram msg)
        {
            return DispatchTime.CompareTo(msg.DispatchTime);
        }
    }


    public class MessageDispatcher
    {
        private List<Telegram> PriorityQ;

        private void DisCharge(BaseGameEntity receiver, Telegram msg)
        {
            if (!receiver.HandleMessage(msg))
            {
                // 报错
            }
        }

        private static MessageDispatcher m_instance;
        public static MessageDispatcher GetInstance()
        {
            return m_instance;
        }

        public MessageDispatcher()
        {
            if (m_instance == null)
            {
                m_instance = this;
                PriorityQ = new List<Telegram>();
            }
        }

        public void DispatchMessage(double delay, int senderID, int receiverID, message_type msg)
        {
            BaseGameEntity receiver = EntityManager.GetInstance().GetEntityFromID(receiverID);
            if (receiver == null)
                return;

            Telegram telegram = new Telegram(senderID, receiverID, msg, delay);
            // 如果不存在延迟，立即发送telegram
            if (delay <= 0)
                DisCharge(receiver, telegram); // 发送telegram到接收器
            // 否则，计算telegram应该被发送的时间
            else
            {
                double CurrentTime = Clock.GetCurrentTime();
                telegram.DispatchTime = CurrentTime + delay;
                PriorityQ.Add(telegram);
            }
        }

        public void DispatchDelayedMessages()
        {
            double CurrentTime = Clock.GetCurrentTime();
            bool flag = (PriorityQ.Count > 0);
            // 遍历删除符合条件的元素
            while (flag)
            {
                int i = 0;
                // 在foreach里删除元素就是个错误
                for (;i < PriorityQ.Count; i++)
                {
                    BaseGameEntity receiver = EntityManager.GetInstance().GetEntityFromID(PriorityQ[i].Receiver);
                    if (PriorityQ[i].DispatchTime >= CurrentTime)
                    {
                        DisCharge(receiver, PriorityQ[i]);
                        PriorityQ.Remove(PriorityQ[i]);
                        break;
                    }
                }
                if (i == PriorityQ.Count)
                    flag = false;
            }
        }
    }
}