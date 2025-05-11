using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Es.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Repository.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired(true);

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.SetNull); // if Brand deleted -> set it as null

            builder.HasOne(p => p.Type)
                .WithMany()
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.TypeId).IsRequired(false);

            builder.Property(p => p.BrandId).IsRequired(false);
        }
    }
}
