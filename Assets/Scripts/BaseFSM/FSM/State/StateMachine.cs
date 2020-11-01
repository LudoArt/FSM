using UnityEngine;

namespace BaseFSM
{
    public class StateMachine<entity_type> : MonoBehaviour
    {
        private entity_type m_Owner;
        private State<entity_type> m_CurrentState;
        private State<entity_type> m_PreviousState;
        private State<entity_type> m_GlobalState;

        public StateMachine(entity_type owner)
        {
            m_Owner = owner;
            m_CurrentState = null;
            m_PreviousState = null;
            m_GlobalState = null;
        }

        #region 使用这些方法来初始化FSM

        public void SetCurrentState(State<entity_type> state)
        {
            m_CurrentState = state;
        }

        public void SetPreviousState(State<entity_type> state)
        {
            m_PreviousState = state;
        }

        public void SetGlobalState(State<entity_type> state)
        {
            m_GlobalState = state;
        }

        #endregion

        public void CustomUpdate()
        {
            if (m_GlobalState != null)
                m_GlobalState.Execute(m_Owner);
            if (m_CurrentState != null)
                m_CurrentState.Execute(m_Owner);
        }

        /// <summary>
        /// 改变到一个新的状态
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(State<entity_type> newState)
        {
            if (m_CurrentState == null || newState == null)
                return;

            m_PreviousState = m_CurrentState;
            m_CurrentState.Exit(m_Owner);
            m_CurrentState = newState;
            m_CurrentState.Enter(m_Owner);
        }

        /// <summary>
        /// 改变状态回到前一个状态
        /// </summary>
        public void RevertToPreviousState()
        {
            ChangeState(m_PreviousState);
        }

        #region 访问

        public State<entity_type> CurrentState()
        {
            return m_CurrentState;
        }
        public State<entity_type> PreviousState()
        {
            return m_PreviousState;
        }
        public State<entity_type> GlobalState()
        {
            return m_GlobalState;
        }

        #endregion

        /// <summary>
        /// 如果当前的状态类型等于传递过来的类的类型，返回true
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public bool IsInState(State<entity_type> st)
        {
            if (m_CurrentState == st)
                return true;
            else
                return false;
        }
    }
}