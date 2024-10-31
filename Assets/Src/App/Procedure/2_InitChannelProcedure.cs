using Framework;
using UnityEngine;

namespace App
{
    public class InitChannelProcedure :  ProdureBase<AppEntry>
    {
        public override bool IsComplete { get; protected set; }
        
        public override void Enter()
        {
            Debug.Log("【Procedure 2 InitChannelProcedure isEnter!】");
            
            IsComplete = true;
        }

        public override void Tick(float detlaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}