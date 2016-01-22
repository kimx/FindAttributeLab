using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FindAttributeLab
{
    public static class AssemblyHelper
    {
        //快取結果，同一個Type下不會重覆讀取
        private static Dictionary<Type, IEnumerable<Type>> DerivedTypes = new Dictionary<Type, IEnumerable<Type>>();

        /// <summary>
        /// 取得所需組件
        /// </summary>
        /// <param name="containGAC">是否要包含GAC組件</param>
        /// <param name="finder">過濾函式</param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssemblies(bool containGAC = false, Func<Assembly, bool> finder = null)
        {
            var app = AppDomain.CurrentDomain;
            var dynamicDirectory = app.SetupInformation.CachePath ?? string.Empty;
            var list = new List<Assembly>();
            foreach (var assembly in app.GetAssemblies())
            {
                if (assembly.IsDynamic || !assembly.Location.Contains(dynamicDirectory))
                {
                    //因為有可能會ShadowCopy
                    continue;
                }

                if (!containGAC && assembly.GlobalAssemblyCache)
                {
                    continue;
                }

                if (finder != null && !finder(assembly))
                {
                    continue;
                }

                list.Add(assembly);
            }

            return list;
        }

        /// <summary>
        /// 尋找所有衍生Type
        /// </summary>
        /// <param name="baseType">基礎Type</param>
        /// <param name="containGAC">是否要包含GAC組件</param>
        /// <param name="findNonPublic">是否要包含私有Type</param>
        /// <returns></returns>
        public static IEnumerable<Type> FindDerivedTypes(Type baseType, bool containGAC = false, bool findNonPublic = false)
        {
            lock (DerivedTypes)
            {
                IEnumerable<Type> derived;
                if (!DerivedTypes.TryGetValue(baseType, out derived))
                {
                    var list = new List<Type>();
                    foreach (var assembly in GetAssemblies(containGAC))
                    {
                        foreach (var type in assembly.GetTypes())
                        {
                            if (type == baseType)
                            {
                                continue;
                            }

                            if (type.IsNotPublic || type.IsNestedPrivate)
                            {
                                if (!findNonPublic)
                                {
                                    continue;
                                }
                            }

                            if (baseType.IsAssignableFrom(type))
                            {
                                list.Add(type);
                            }
                        }
                    }
                    derived = list;
                    DerivedTypes.Add(baseType, list);
                }

                return derived;
            }
        }


        public static Type GetTypeByName(string assemblyFullName, string classFullName)
        {
            return Type.GetType(GetTypeName(assemblyFullName, classFullName));
        }

        public static string GetTypeName(string assemblyFullName, string classFullName)
        {
            string typeName = string.Format("{0}, {1}", classFullName, assemblyFullName);
            return typeName;
        }
    }
}
