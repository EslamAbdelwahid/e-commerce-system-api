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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
                .IsRequired()
                .HasDefaultValue(OrderStatus.Pending)
                .HasConversion(
                v => v.ToString(), // enum to string if you want to store in Db
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v)); // string to enum if you want to get date

            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.OwnsOne(o => o.ShippingAddress, address =>
            {
                address.WithOwner(); // if your Address class has a navigation property back to its owner (Order), you should use .WithOwner() to tell EF Core about that relationship.
                address.Property(add => add.Country).IsRequired().HasMaxLength(55);
                address.Property(add => add.FirstName).IsRequired().HasColumnName("ShippingFirstName");
                address.Property(add => add.LastName).IsRequired().HasColumnName("ShippingLastName");

            });

            builder.OwnsOne(o => o.ShippingAddress, sa => sa.WithOwner());

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .HasForeignKey(o => o.DeliveryMethodId);



        }
    }
}
