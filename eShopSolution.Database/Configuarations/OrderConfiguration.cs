using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolutions.Database.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configuarations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);  

            builder.Property(x=>x.ShipEmail).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.ShipAddress).IsRequired().HasMaxLength(200);


            builder.Property(x => x.ShipName).IsRequired().HasMaxLength(200);


            builder.Property(x => x.ShipPhoneNumber).IsRequired().HasMaxLength(200);

        }
    }
}
