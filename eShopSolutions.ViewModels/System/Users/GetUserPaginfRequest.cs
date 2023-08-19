using eShopSolutions.ViewModels.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.System.Users
{
    public class GetUserPaginfRequest : PagingRequestBase
    {
       public String KeyWord { get; set; }
    }
}
