﻿using eShopSolutions.ViewModels.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.Catalog.Products.Public
{
    public class GetProductPagingRequest : PagingRequestBase
    {
      public string KeyWord { get; set; }  
        public List<int> CategoryId { get; set; }
    }
}
    