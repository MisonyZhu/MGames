using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 流程模块一般是线性的，前后流程间有一定的依赖关系
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Produrer<T> : IProdureManager<T>,IDisposable
    {
        private T m_Owner;
        
        private LinkedList<ProdureBase<T>> m_Produrecs = new LinkedList<ProdureBase<T>>();

        private LinkedListNode<ProdureBase<T>> m_CurNode;
        
        private ProdureBase<T> m_CurProdure;

        private Dictionary<string, object> m_BlackBoard = new();

        public T Owner => m_Owner;
        public bool IsComplete { get; private set; }
        public Action OnComplete;

        public Produrer(T owner)
        {
            m_Owner = owner;
        }

        public void Update(float detlaTime)
        {
            m_CurProdure?.Tick(detlaTime);
            if (m_CurProdure!=null)
            {
                if (m_CurProdure.MoveNext())
                {
                    NextProdurce();
                }
            }
        }

        public void RunProdurce()
        {
            if (m_Produrecs.Count < 0)
            {
                throw new Exception("Produrer no have sub produrer!");
            }

            IsComplete = false;
            m_CurNode = m_Produrecs.First;
            m_CurNode.Value.Enter();
            m_CurProdure = m_CurNode.Value;
        }

        public void Reset()
        {
            m_BlackBoard.Clear();
            m_CurNode = null;
            m_CurProdure = null;
        }

        void NextProdurce()
        {
            if (m_CurProdure != null)
            {
                m_CurProdure.Exit();
                if (m_CurNode.Next != null)
                {
                    m_CurNode = m_CurNode.Next;
                    m_CurNode.Value.Enter();
                    m_CurProdure = m_CurNode.Value;
                }
                else
                {
                    OnComplete?.Invoke();
                    IsComplete = true;
                    Reset();
                }
            }
            else
            {
                throw new Exception("Produrer is not start or has over,please check");
            }
        }


        #region Add

        public void Add<TState>() where TState : ProdureBase<T>
        {
            var node = Activator.CreateInstance(typeof(TState)) as ProdureBase<T>;
            if (!m_Produrecs.Contains(node))
            {
                m_Produrecs.AddLast(node);
            }
            else
            {
                throw new Exception($"Produrer {node.GetType()} has exit!");
            }
        }

        public void Add<TState>(TState state) where TState : ProdureBase<T>
        {
            var node = Activator.CreateInstance(state.GetType()) as ProdureBase<T>;
            if (!m_Produrecs.Contains(node))
            {
                m_Produrecs.AddLast(node);
            }
            else
            {
                throw new Exception($"Produrer {node.GetType()} has exit!");
            }
        }

        #endregion

        public void Dispose()
        {
            if (m_CurProdure != null)
            {
                m_CurProdure.Exit();
            }
           
            Reset();
            m_BlackBoard.Clear();
            m_CurProdure = null;
            
        }

        #region BlackBoard

        public void SetBlackBoardValue(string key, object value)
        {
            if (m_BlackBoard.ContainsKey(key))
            {
                m_BlackBoard[key] = value;
            }
            else
            {
                m_BlackBoard.Add(key, value);
            }
        }

        public object GetBlackBoardValue(string key)
        {
            m_BlackBoard.TryGetValue(key, out object result);
            return result;
        }

        #endregion
    }
}