using eShopSolutions.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Database.Configuarations
{
    public class AppConfigConfigguration : IEntityTypeConfiguration<AppConfig>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AppConfig> builder)
        {
            builder.ToTable("AppConfigs"); 
            builder.HasKey(x => x.Key); 
            builder.Property(x =>x.Value).IsRequired(true);
        }
    }
}
