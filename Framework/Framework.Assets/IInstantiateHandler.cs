using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    public interface IInstantiateHandler : IDisposable
    {
        public  GameObject Result { get; }
    
        void Cancel();

        void WaitForAsyncComplete();
    }
}
