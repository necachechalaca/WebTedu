using eShopSolution.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Database.Configuarations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(150); 
            builder.Property(x=>x.LastName).IsRequired().HasMaxLength(150); 
        }
    }
}
