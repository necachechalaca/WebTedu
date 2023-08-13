using Microsoft.AspNetCore.Identity;
using System;

namespace eShopSolution.Database.Entity
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; } 
    }
}
