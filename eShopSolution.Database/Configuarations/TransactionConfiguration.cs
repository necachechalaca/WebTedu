using eShopSolutions.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace eShopSolution.Data.Configuarations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transactionn>
    {
        public void Configure(EntityTypeBuilder<Transactionn> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.AppUser).WithMany(x => x.Transactionns).HasForeignKey(x => x.UserID);
        }


    }
    
}
