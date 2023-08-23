using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.Common.Dtos
{
    public class ApiSuccessResult<T> : ApiResult<T>

    {

        public ApiSuccessResult(T resultObj)
        {
            IsSuccess = true;  
            ResultObj = resultObj;  

        }
        public ApiSuccessResult()
        {
            IsSuccess =true;   
        }   
    }
}
