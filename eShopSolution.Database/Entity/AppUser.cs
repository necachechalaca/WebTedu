using eShopSolutions.Database.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace eShopSolution.Database.Entity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Cart> Carts { get; set; }   

        public List<Order> Orders { get; set; } 

        public List<Transactionn> Transactionns { get; set; }   

    }
}
