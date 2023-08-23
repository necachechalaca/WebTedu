using eShopSolutions.ViewModels.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nest;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace eShopSolutions.AdminApp.Controllers.Components
{
    public class PagerViewComPonent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResult resultBase)
        {
            return Task.FromResult((IViewComponentResult)View("Default", resultBase));
        }
    }
}
