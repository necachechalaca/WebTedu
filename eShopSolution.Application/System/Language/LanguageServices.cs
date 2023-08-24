using eShopSolutions.Database.EF;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.System.Language
{
    public class LanguageServices : ILanguageServices
    {
        private readonly IConfiguration _configuration;
        private readonly EShopDbContext _context;
        public LanguageServices(IConfiguration configuration, EShopDbContext eShopDbContext)
        {
            _configuration = configuration; 
            _context = eShopDbContext;
        }
        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            var languages =  await _context.Languages.Select(x => new LanguageVm() 
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();

            return new ApiSuccessResult<List<LanguageVm>>(languages);
        }
    }
}
