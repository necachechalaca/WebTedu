using eShopSolutions.Database.Entity;
using eShopSolutions.ViewModels.Catalog.ProductImage;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Utilities.SlideViewModels;
using System.Collections.Generic;

namespace eShopSolutions.Webapp.Models
{
    public class HomeViewModels
    {
        public List<SlideViewModels> Slides { get; set; }

        public List<ProductViewModel> FeaturedProducts { get; set; }
    }
}
