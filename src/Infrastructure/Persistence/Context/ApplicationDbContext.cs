using Finbuckle.MultiTenant;
using FSH.WebApi.Application.Accounting;
using FSH.WebApi.Application.Common.Events;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Catalog;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.General;
using FSH.WebApi.Domain.Helper;
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
    public DbSet<Picture> Picture => Set<Picture>();
    public DbSet<Salutation> Salutation => Set<Salutation>();
    public DbSet<Country> Country => Set<Country>();
    public DbSet<Person> Person => Set<Person>();
    public DbSet<Company> Company => Set<Company>();
    public DbSet<VipStatus> VipStatus => Set<VipStatus>();
    public DbSet<StateRegion> StateRegion => Set<StateRegion>();
    public DbSet<PriceTagDetail> PriceTagDetail => Set<PriceTagDetail>();
    public DbSet<PriceTag> PriceTag => Set<PriceTag>();
    public DbSet<Reservation> Reservation => Set<Reservation>();
    public DbSet<PackageExtend> PackageExtend => Set<PackageExtend>();
    public DbSet<MandantNumbers> MandantNumbers => Set<MandantNumbers>();
    public DbSet<MandantSetting> MandantSetting => Set<MandantSetting>();
    public DbSet<VCat> VCat => Set<VCat>();
    public DbSet<RoomReservation> RoomReservation => Set<RoomReservation>();
    public DbSet<Booking> Booking => Set<Booking>();
    public DbSet<Journal> Journal => Set<Journal>();
    public DbSet<ItemPriceTax> ItemPriceTax => Set<ItemPriceTax>();
    public DbSet<MandantDetail> MandantDetail => Set<MandantDetail>();
    public DbSet<Invoice> Invoice => Set<Invoice>();
    public DbSet<InvoiceDetail> InvoiceDetail => Set<InvoiceDetail>();
    public DbSet<CashierRegister> CashierRegister => Set<CashierRegister>();
    public DbSet<CashierJournal> CashierJournal => Set<CashierJournal>();
    public DbSet<Appointment> Appointment => Set<Appointment>();
    public DbSet<Board> Board => Set<Board>();
    public DbSet<BoardItemLabel> BoardItemLabel => Set<BoardItemLabel>();
    public DbSet<BoardCollection> BoardCollection => Set<BoardCollection>();
    public DbSet<BoardItemAttachment> BoardItemAttachment => Set<BoardItemAttachment>();
    public DbSet<BoardItemSub> BoardItemSub => Set<BoardItemSub>();
    public DbSet<BoardItem> BoardItem => Set<BoardItem>();
    public DbSet<BoardItemTagGroup> BoardItemTagGroup => Set<BoardItemTagGroup>();
    public DbSet<BoardItemTag> BoardItemTag => Set<BoardItemTag>();
    public DbSet<BoardItemLabelGroup> BoardItemLabelGroup => Set<BoardItemLabelGroup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Lnx);
    }
}