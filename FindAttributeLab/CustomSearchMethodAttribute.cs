using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAttributeLab
{
    public class CustomSearchMethodAttribute : Attribute
    {
        public CustomSearchMethodAttribute(string name, string descript = null)
        {
            this.Name = name;
            this.Descript = descript;
        }
        public string Name { get; set; }

        public string Descript { get; set; }
    }
}
