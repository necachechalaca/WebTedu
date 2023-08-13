using eShopSolutions.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configuarations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Productt>
    {
        public void Configure(EntityTypeBuilder<Productt> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Price).IsRequired();    
            builder.Property(x => x.Stock).IsRequired().HasDefaultValue(0); 
            builder.Property(x =>x.ViewCount).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.OriginalPrice).IsRequired();

        }
    }
}
