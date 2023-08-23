using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.Common.Dtos
{
    public class ApiErorResult<T> : ApiResult<T>
    {
        public string[] ValidationErors { get; set; }   
        public ApiErorResult() { }
        public ApiErorResult(string messge)
        {
            IsSuccess = false;
            Message = messge;
        }
        public ApiErorResult(string[] validationErors) 
        {
            IsSuccess = false;
            ValidationErors = validationErors;
        }
    }
}
