using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Ecommerce.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Id).IsRequired() ;
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).IsRequired();
        builder.Property(p => p.Price).HasColumnType("decimal");
        builder.Property(p => p.PictureUrl).IsRequired();

        // Disable cascade delete on foreign key relationships to avoid multiple cascade paths
        builder.HasOne(p => p.ProductBrand)
            .WithMany()
            .HasForeignKey(p => p.ProductBrandId)
            .OnDelete(DeleteBehavior.Restrict);  // Disables cascade delete

        //builder.HasOne(p => p.ProductType)
        //    .WithMany()
        //    .HasForeignKey(p => p.ProductTypeId)
        //    .OnDelete(DeleteBehavior.Restrict);  // Disables cascade delete

        builder.HasOne(p => p.ProductSubType)
            .WithMany()
            .HasForeignKey(p => p.ProductSubTypeId)
            .OnDelete(DeleteBehavior.Restrict);  // Disables cascade delete
    }
}
