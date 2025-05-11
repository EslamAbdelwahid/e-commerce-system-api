using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Es.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Repository.Data.Configurations.OrderModuleConfigurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Price).HasColumnType("decimal(18,2)");
            builder.OwnsOne(oi => oi.Product, sa => sa.WithOwner());

            //builder.HasOne<Order>()         // Each OrderItem belongs to one Order
            //       .WithMany()               // An Order has many OrderItems
            //       .HasForeignKey(oi => oi.OrderId) // Use explicit FK
            //       .OnDelete(DeleteBehavior.Cascade); // to delete Items if you want to delte the order (auto)
        }
    }
}
