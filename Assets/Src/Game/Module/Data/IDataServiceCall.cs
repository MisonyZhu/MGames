using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface IDataServiceCall
    {
        void OnLogin();

        void OnLoginOut();

        void OnReConnect();

        void OnDisConnect();
    }
}
