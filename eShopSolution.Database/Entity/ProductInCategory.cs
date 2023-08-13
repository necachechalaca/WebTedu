using eShopSolutions.Database.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Database.Entity
{
    public class ProductInCategory
    {
        public int ProductId { get; set; } /*ForeignKey*/

        public Productt Product { get; set; }


        public int CategoryId { get; set; }  /*ForeignKey*/
        public Category Category { get; set; }
    }
}
