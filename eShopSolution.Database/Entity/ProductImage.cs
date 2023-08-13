using System;

namespace eShopSolutions.Database.Entity
{
    public class ProductImage
    {
        public int Id { get; set; } 

        public int ProductId { get; set; }  

        public string ImagePath { get; set; }   

        public string Caption { get;set; }

        public bool IsDefault { get; set; }

        public DateTime DateCreated { get; set; }   

        public int SortOder { get; set; }

        public long FileSize { get; set; }

        public Productt Productt { get; set; }  
    }
}
