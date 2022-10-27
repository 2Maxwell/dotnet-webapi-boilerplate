using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;


namespace FSH.WebApi.Application.Hotel.Packages;

public class CreatePackageRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? Display { get; set; }
    public int InvoicePosition { get; set; } = 1;
    public string? InvoiceName { get; set; }
    public int PackageBookingBaseEnumId { get; set; }
    public int PackageBookingRhythmEnumId { get; set; }
    public bool Optional { get; set; }
    public bool ShopExtern { get; set; }
    public List<PackageItemDto>? PackageItems { get; set; }

}

public class CreatePackageRequestValidator : CustomValidator<CreatePackageRequest>
{
    public CreatePackageRequestValidator(IReadRepository<Package> repository, IStringLocalizer<CreatePackageRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50)
        .MustAsync(async (package, name, ct) => await repository.GetBySpecAsync(new PackageByNameSpec(name, package.MandantId), ct) is null)
        .WithMessage((_, name) => string.Format(localizer["packageName.alreadyexists"], name));

        RuleFor(x => x.Kz)
        .NotEmpty()
        .MaximumLength(10)
        .MustAsync(async (package, kz, ct) => await repository.GetBySpecAsync(new PackageByKzSpec(kz, package.MandantId), ct) is null)
        .WithMessage((_, kz) => string.Format(localizer["packageKz.alreadyexists"], kz));

        RuleFor(x => x.Description)
        .MaximumLength(200);
        RuleFor(x => x.Display)
        .MaximumLength(50);
    }
}

public class PackageByNameSpec : Specification<Package>, ISingleResultSpecification
{
    public PackageByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class PackageByKzSpec : Specification<Package>, ISingleResultSpecification
{
    public PackageByKzSpec(string kz, int mandantId) =>
        Query.Where(x => x.Kz == kz && (x.MandantId == mandantId || x.MandantId == 0));
}

public class PackageDtoByKzSpec : Specification<Package, PackageDto>, ISingleResultSpecification
{
    public PackageDtoByKzSpec(string kz, int mandantId) =>
        Query.Where(x => x.Kz == kz && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreatePackageRequestHandler : IRequestHandler<CreatePackageRequest, int>
{
    private readonly IRepository<Package> _repository;
    private readonly IRepository<PackageItem> _packageItemRepository;
    public CreatePackageRequestHandler(IRepository<Package> repository, IRepository<PackageItem> packageItemRepository)
    {
        _repository = repository;
        _packageItemRepository = packageItemRepository;
    }

    public async Task<int> Handle(CreatePackageRequest request, CancellationToken cancellationToken)
    {
        var package = new Package(
            request.MandantId,
            request.Name,
            request.Kz,
            request.Description,
            request.Display,
            request.InvoicePosition,
            request.InvoiceName,
            request.PackageBookingBaseEnumId,
            request.PackageBookingRhythmEnumId,
            request.Optional,
            request.ShopExtern);
        package.DomainEvents.Add(EntityCreatedEvent.WithEntity(package));
        await _repository.AddAsync(package, cancellationToken);

        if (request.PackageItems != null && request.PackageItems.Count() > 0)
        {
            foreach (PackageItemDto packageItemDto in request.PackageItems)
            {
                PackageItem p = new PackageItem();
                p.Start = (DateTime)packageItemDto.Start;
                p.End = packageItemDto.End;
                p.ItemId = packageItemDto.ItemId;
                p.Price = packageItemDto.Price;
                p.Percentage = packageItemDto.Percentage;
                p.PackageId = package.Id;
                p.PackageItemCoreValueEnumId = packageItemDto.PackageItemCoreValueEnumId;
                p.DomainEvents.Add(EntityCreatedEvent.WithEntity(p));
                await _packageItemRepository.AddAsync(p, cancellationToken);
            }
        }

        return package.Id;
    }
}