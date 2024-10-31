using System.Collections;

namespace Framework
{
    public abstract class ProdureBase<T> : IEnumerator
    {
        public Produrer<T> Produre { get; internal set; }
        public abstract bool IsComplete { get; protected set; }


        public abstract void Enter();

        public abstract void Tick(float detlaTime);


        public abstract void Exit();


        #region Aysnc

        public bool MoveNext()
        {
            return IsComplete;
        }

        public void Reset()
        {
        }

        public object Current
        {
            get { return this; }
        }

        #endregion
    }
}