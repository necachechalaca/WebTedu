using eShopSolutions.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.Utilities.SlideViewModels
{
    public class SlideViewModels
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public int SortOder { get; set; }
       
    }
}
