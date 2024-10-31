namespace Framework
{
    public abstract class FsmBaseNode 
    {
        public Fsm Fsm { get; internal set; }

        public virtual void Enter()
        {
           
        }

        public virtual void Update(float detlaTime)
        {
           
        }

        public virtual void Exit()
        {
            
        }

        internal virtual void Reset()
        {
           
        }
        
          
    }
}