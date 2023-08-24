using eShopSolutions.AdminApp.Models;
using eShopSolutions.Application.System.Users;
using eShopSolutions.Utilities.Contains;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolutions.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiClient _languageApiClient;
        public NavigationViewComponent(ILanguageApiClient languageApiClient)
        {
            _languageApiClient = languageApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languague = await _languageApiClient.GetALL();

            var navigation = new NavigationViewModel()
            {
                CurrentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId),
                Languages = languague.ResultObj
            };



            return View("Default", navigation);
        }
    }
}
