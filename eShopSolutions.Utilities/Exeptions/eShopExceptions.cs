using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Utilities.Exeptions
{
    public class eShopExceptions : Exception
    {
        public eShopExceptions()
        {
        }

        public eShopExceptions(string message)
            : base(message)
        {
        }

        public eShopExceptions(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
