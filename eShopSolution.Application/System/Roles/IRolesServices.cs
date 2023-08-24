using eShopSolutions.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.System.Roles
{
    public interface IRolesServices
    {
        Task<List<RolesViewModels>> GetAll();
    }
}
