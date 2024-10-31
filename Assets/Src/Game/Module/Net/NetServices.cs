using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Net;

namespace Game
{
    public static class NetServices
    {
        private static List<Type> netServices = new List<Type>();
        
        public static void Initialize()
        {
            netServices.Clear();
            ExploreImplementations(typeof(NetServiceBase), ref netServices);

            foreach (var netService in netServices)
            {
               var service = (NetServiceBase)Activator.CreateInstance(netService);
               if (service != null)
               {
                   service.RegisterEvents();
                   NetModule.Instance.RegisterService(service);
               }
            }
        }


        // 导出所有子类型
        public static void ExploreImplementations(Type baseType, ref List<Type> implementations)
        {
            if (implementations == null) return;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (baseType.IsAssignableFrom(type) &&
                        !type.IsAbstract &&
                        !type.IsInterface &&
                        type.IsSubclassOf(baseType))
                    {
                        implementations.Add(type);
                    }
                }
            }
        }
    }
}