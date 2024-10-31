using System;

namespace Framework
{
    public interface IFsm
    {
        void AddStateNode<T>(T state);

        void AddStateNode<T>() where T : FsmBaseNode;

        void RunStateNode<T>() where T : FsmBaseNode;

        void ChangeState<T>() where T : FsmBaseNode;
        
        FsmBaseNode GetCurState<T>() where T : FsmBaseNode;

        void ResetStates();

        void Tick(float detlaTime);

        void Pause(bool isPause);
        
         void Dispose();
    }
}