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
    public class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.ToTable("ProductInCategories");
            builder.HasKey(x => new { x.ProductId, x.CategoryId });
            

            builder.HasOne(t =>t.Product).WithMany(p => p.ProductInCategories).HasForeignKey(t => t.ProductId);

            builder.HasOne(t => t.Category).WithMany(p => p.ProductInCategories).HasForeignKey(t => t.CategoryId);
        }
    }
}
