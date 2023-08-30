using eShopSolutions.ViewModels.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.Catalog.Products
{
    public class CategoryAssignRequest
    {
        public int Id { get; set; } 

        public List<SelectItem> Categories { get; set; } = new List<SelectItem>();
    }
}
