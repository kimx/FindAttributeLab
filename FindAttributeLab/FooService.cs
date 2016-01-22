using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAttributeLab
{
    public class FooService
    {
        [CustomSearchMethod("CustomSearchOrder", "自訂查詢-訂單")]
        public void CustomSearchOrder()
        {

        }

        [CustomSearchMethod("CustomSearchShip", "自訂查詢-出貨")]
        public void CustomSearchShip()
        {

        }

        public void CustomSearchNoAttribute()
        {

        }
    }
}
