using eShopSolutions.Database.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Database.Entity
{

    public class Productt
    {

        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public bool? IsFeatured { get; set; }   

        public int ViewCount { get; set; }
        public DateTime DateCreated { get; set; }
        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Cart> Carts { get; set; }

        public List<ProductTranslation> ProductTranslations { get; set; }
        
        public List<ProductImage> ProductImages { get; set; }   

    }
}
