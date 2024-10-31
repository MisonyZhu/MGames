using System;
using System.Collections.Generic;

namespace Framework
{
     public class Fsm : IFsm
    {
        private object m_Owner;

        private readonly Dictionary<Type, FsmBaseNode> m_States;

        private FsmBaseNode m_CurState;

        public IFsmData BlackBoardData { get; private set; }
        public bool IsPause { get; private set; }

        public object GetOwner => m_Owner;

        Fsm(object owner)
        {
            m_States = new Dictionary<Type, FsmBaseNode>();
            m_Owner = owner;
        }

        public static IFsm CreateFsm(object owner)
        {
            Fsm fsm = new Fsm(owner);
            fsm.BlackBoardData = new FsmData();
            return fsm;
        }

        public void Tick(float detlaTime)
        {
            if (IsPause)
                return;

            m_CurState?.Update(detlaTime);
        }

        #region Add

        public void AddStateNode<T>(T state)
        {
            if (!m_States.TryGetValue(state.GetType(), out FsmBaseNode node))
            {
                node = state as FsmBaseNode;
                node.Fsm = this;
                m_States.Add(state.GetType(), node);
            }
            else
            {
                throw new Exception($"Framework error FSM State type has exit.");
            }
        }

        public void AddStateNode<T>() where T : FsmBaseNode
        {
            Type state = typeof(T);
            if (!m_States.TryGetValue(state, out FsmBaseNode node))
            {
                node = Activator.CreateInstance(state) as FsmBaseNode;
                node.Fsm = this;
                m_States.Add(state, node);
            }
            else
            {
                throw new Exception($"Framework error FSM State type has exit.");
            }
        }

        #endregion

        #region Run Pause

        public void RunStateNode<T>() where T : FsmBaseNode
        {
            var state = typeof(T);
            m_States.TryGetValue(state, out FsmBaseNode nextState);
            if (nextState == null)
            {
                throw new Exception($"Framework error FSM State type is not exit.");
            }

            if (m_CurState != null)
            {
                m_CurState.Exit();
            }

            nextState.Enter();
            m_CurState = nextState;
        }

        public void Pause(bool isPause)
        {
            IsPause = isPause;
        }

        public void Dispose()
        {
            m_CurState = null;
            IsPause = false;
            m_States.Clear();
        }

        #endregion

        #region Change

        public void ChangeState<T>() where T : FsmBaseNode
        {
            var state = typeof(T);
            m_States.TryGetValue(state, out FsmBaseNode nextState);
            if (nextState == null)
            {
                throw new Exception($"Framework error FSM State type is not exit.");
            }

            if (m_CurState != null)
            {
                m_CurState.Exit();
            }

            nextState.Enter();
            m_CurState = nextState;
        }

        public FsmBaseNode GetCurState<T>() where T : FsmBaseNode
        {
            return  m_CurState;
        }

        #endregion

        #region Reset

        public void ResetStates()
        {
            m_CurState = null;
            IsPause = false;

            var ie = m_States.GetEnumerator();
            while (ie.MoveNext())
            {
                ie.Current.Value.Reset();
            }
        }

        #endregion

       
    }
}