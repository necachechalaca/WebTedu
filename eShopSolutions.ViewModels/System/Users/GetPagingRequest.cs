using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.System.Users
{
    public class GetPagingRequest
    {
        public String KeyWord { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
