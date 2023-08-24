using eShopSolutions.ViewModels.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.System.Users
{
    public class RolesAssignRequest
    {
        public Guid Id { get; set; }
        public List<SelectItem>Roles { get; set; }  = new List<SelectItem>();

    }
}
