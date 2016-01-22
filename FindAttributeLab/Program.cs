using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAttributeLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Console.WriteLine("");
            var assemblys = AssemblyHelper.GetAssemblies();
            foreach (var assembly in assemblys)
            {
                // Console.WriteLine($"assembly:{assembly.FullName}");
                var publicClasses = assembly.DefinedTypes;
                foreach (var publicClass in publicClasses)
                {
                   // Console.WriteLine($"publicClass:{publicClass.Name}");
                    var methods = publicClass.GetMethods();
                    foreach (var method in methods)
                    {
                      //  Console.WriteLine($"method:{method.Name}");
                        var attrs = method.GetCustomAttributes(typeof(CustomSearchMethodAttribute), false);
                        foreach (var  attr in attrs)
                        {
                            CustomSearchMethodAttribute custAttr = (CustomSearchMethodAttribute)attr;
                            Console.WriteLine($"assembly:{assembly.FullName}");
                            Console.WriteLine($"publicClass:{publicClass.FullName}");
                            Console.WriteLine($"Name:{custAttr.Name} , Descript:{custAttr.Descript}");
                        }
                    }
                }
            }
            Console.WriteLine("");
            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
