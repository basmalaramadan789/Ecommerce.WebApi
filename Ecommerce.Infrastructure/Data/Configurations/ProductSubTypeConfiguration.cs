using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductSubTypeConfiguration : IEntityTypeConfiguration<ProductSubType>
{
    public void Configure(EntityTypeBuilder<ProductSubType> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Disable cascade delete on ProductType relationship to avoid multiple cascade paths
        builder.HasOne(p => p.ProductType)
            .WithMany(t => t.ProductSubTypes)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);  // Disables cascade delete
    }
}
