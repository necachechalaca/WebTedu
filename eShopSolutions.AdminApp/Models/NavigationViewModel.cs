using eShopSolutions.ViewModels.System.ViewModel;
using System.Collections.Generic;

namespace eShopSolutions.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<LanguageVm> Languages { get; set; } 

        public string CurrentLanguageId { get; set; } 
    }
}
