using Finbuckle.MultiTenant;
using FSH.WebApi.Application.Accounting;
using FSH.WebApi.Application.Common.Events;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Catalog;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FSH.WebApi.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Language> Language => Set<Language>();
    public DbSet<PluGroup> PluGroup => Set<PluGroup>();
    public DbSet<Merchandise> Merchandise => Set<Merchandise>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Room> Room => Set<Room>();
    public DbSet<Package> Package => Set<Package>();
    public DbSet<Mandant> Mandant => Set<Mandant>();
    public DbSet<ItemGroup> ItemGroup => Set<ItemGroup>();
    public DbSet<Item> Item => Set<Item>();
    public DbSet<Period> Period => Set<Period>();
    public DbSet<PackageItem> PackageItem => Set<PackageItem>();
    public DbSet<PriceSchema> PriceSchema => Set<PriceSchema>();
    public DbSet<PriceSchemaDetail> PriceSchemaDetail => Set<PriceSchemaDetail>();
    public DbSet<BookingPolicy> BookingPolicy => Set<BookingPolicy>();
    public DbSet<CancellationPolicy> CancellationPolicy => Set<CancellationPolicy>();
    public DbSet<Rate> Rate => Set<Rate>();
    public DbSet<PriceCat> PriceCat => Set<PriceCat>();
    public DbSet<Tax> Tax => Set<Tax>();
    public DbSet<TaxItem> TaxItem => Set<TaxItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Lnx);
    }
}