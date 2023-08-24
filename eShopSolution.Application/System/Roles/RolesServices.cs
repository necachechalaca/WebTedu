using eShopSolution.Database.Entity;
using eShopSolutions.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.System.Roles
{
    public class RolesServices : IRolesServices
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RolesServices(RoleManager<AppRole> roleManager)
        { 
            _roleManager = roleManager; 
        }
        public async Task<List<RolesViewModels>> GetAll() 
        {
            var roles = await _roleManager.Roles.Select(r => new RolesViewModels 
            {
                Id = r.Id,
                Name = r.Name,  
                Description = r.Description,    

            }).ToListAsync();
            
            return roles;   
        }
    }
}
