using eShopSolutions.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace eShopSolutions.Database.Configuarations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
       
            builder.HasKey(x => x.Id);
            builder.Property(x =>x.Id).UseIdentityColumn();

            builder.Property(x=>x.ImagePath).HasMaxLength(200).IsRequired();

            builder.Property(x => x.Caption).HasMaxLength(200);


            builder.HasOne(x => x.Productt ).WithMany(x=> x.ProductImages).HasForeignKey(x=>x.ProductId);  
        }
    }
}
