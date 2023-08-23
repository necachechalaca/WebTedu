using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.Common.Dtos
{
    public class ApiResult<T>
    {
      
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T ResultObj { get; set; }
    }
}
