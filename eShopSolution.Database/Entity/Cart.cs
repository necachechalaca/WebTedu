using eShopSolution.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eShopSolutions.Database.Entity
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Productt Product { get; set; }

        public Guid UserID { get; set; }

        public AppUser AppUser { get; set; }


    }
}
