using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.Configurations
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure the relationship between ProductType and ProductSubTypes
            builder.HasMany(p => p.ProductSubTypes)
                .WithOne(t => t.ProductType)
                .HasForeignKey(t => t.ProductTypeId)
                .OnDelete(DeleteBehavior.Cascade);  // Only this one will have cascade delete
        }
    }
}