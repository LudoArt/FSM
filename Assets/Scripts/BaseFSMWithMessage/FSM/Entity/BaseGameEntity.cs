
namespace FSMMSG
{
    public abstract class BaseGameEntity
    {
        private int m_ID;
        private string m_sName;
        private static int m_iNextValidID = 0;
        private void SetID(int val)
        {
            if (val >= m_iNextValidID)
            {
                m_ID = val;
                m_iNextValidID = m_ID + 1;
            }
        }
        private void SetName(string name)
        {
            m_sName = name;
        }

        public BaseGameEntity(int id, string name)
        {
            SetID(id);
            SetName(name);
        }

        public virtual void Update()
        {

        }

        public int ID()
        {
            return m_ID;
        }

        public string GetNameOfEntity()
        {
            return m_sName;
        }

        public abstract bool HandleMessage(Telegram msg);
    }
}