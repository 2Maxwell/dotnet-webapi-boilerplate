using Finbuckle.MultiTenant.EntityFrameworkCore;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.WebApi.Infrastructure.Persistence.Configuration;

public class MerchandiseConfig : IEntityTypeConfiguration<Merchandise>
{
    public void Configure(EntityTypeBuilder<Merchandise> builder)
    {
        builder.IsMultiTenant();
    }
}

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Kz)
                .HasMaxLength(10);

        builder
            .Property(b => b.Name)
                .HasMaxLength(100);

        builder
            .Property(b => b.Description)
                .HasMaxLength(500);

        builder
            .Property(b => b.Properties)
                .HasMaxLength(500);

        builder
            .Property(b => b.DisplayDescriptionShort)
                .HasMaxLength(500);

        builder
            .Property(b => b.DisplayHightlights)
                .HasMaxLength(500);

        builder
            .Property(b => b.VirtualCategoryFormula)
                .HasMaxLength(200);


    }
}
