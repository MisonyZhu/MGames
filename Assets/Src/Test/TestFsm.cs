using System;
using Framework;
using UnityEngine;

namespace Test
{
    public interface IITest
    {
        void TTT();
    }
    public class TestFsm : MonoBehaviour
    {
        private IFsm fsm;
        private void Start()
        {
            fsm = Fsm.CreateFsm(this);
        }
    }
}

namespace Test1
{
    public class TestFsm : MonoBehaviour
    {
        private IFsm fsm;
        private void Start()
        {
            fsm = Fsm.CreateFsm(this);
        }
    }

    public class Test2 : TestFsm, Test.IITest
    {
        public void TTT()
        {
            throw new NotImplementedException();
        }
    }
}