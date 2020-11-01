using System.Collections.Generic;
using UnityEngine;

namespace BaseFSM
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

            GoHomeAndSleepTilRested goHomeAndSleepTilRested = new GoHomeAndSleepTilRested();
            EnterMineAndDigForNugget enterMineAndDigForNugget = new EnterMineAndDigForNugget();
            VisitBankAndDepositGold visitBankAndDepositGold = new VisitBankAndDepositGold();
            QuenchThirst quenchThirst = new QuenchThirst();

            m_EntityMap = new Dictionary<int, BaseGameEntity>();
            counter = 0;
        }

        private void Update()
        {
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