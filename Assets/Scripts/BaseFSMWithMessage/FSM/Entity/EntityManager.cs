using System.Collections.Generic;
using UnityEngine;

namespace FSMMSG
{
    public class EntityManager : MonoBehaviour
    {
        private Dictionary<int, BaseGameEntity> m_EntityMap;
        private int counter;

        private static EntityManager m_instance;
        public static EntityManager GetInstance()
        {
            return m_instance;
        }

        private void Start()
        {
            if (m_instance == null)
                m_instance = this;
            else
                return;


            #region 这些都不该写这，搞个GameMananger之类的存着更直观

            GoHomeAndSleepTilRested goHomeAndSleepTilRested = new GoHomeAndSleepTilRested();
            EnterMineAndDigForNugget enterMineAndDigForNugget = new EnterMineAndDigForNugget();
            VisitBankAndDepositGold visitBankAndDepositGold = new VisitBankAndDepositGold();
            QuenchThirst quenchThirst = new QuenchThirst();
            EatStew eatStew = new EatStew();

            WifesGlobalState wifesGlobalState = new WifesGlobalState();
            DoHouseWork doHouseWork = new DoHouseWork();
            VisitBathroom visitBathroom = new VisitBathroom();
            CookStew cookStew = new CookStew();

            MessageDispatcher messageDispatcher = new MessageDispatcher(); 

            #endregion

            m_EntityMap = new Dictionary<int, BaseGameEntity>();
            counter = 0;
        }

        private void Update()
        {
            MessageDispatcher.GetInstance().DispatchDelayedMessages();
            if (counter < 500)
            {
                counter++;
                return;
            }
            else
            {
                counter = 0;
            }

            foreach (var item in m_EntityMap)
            {
                item.Value.Update();
            }
        }

        public void RegisterEntity(BaseGameEntity NewEntity)
        {
            m_EntityMap.Add(NewEntity.ID(), NewEntity);
        }

        public BaseGameEntity GetEntityFromID(int id)
        {
            BaseGameEntity entity;
            if (m_EntityMap.TryGetValue(id, out entity))
                return entity;
            else
                return null;
        }

        public bool RemoveEntity(BaseGameEntity entity)
        {
            return m_EntityMap.Remove(entity.ID());
        }
    }
}