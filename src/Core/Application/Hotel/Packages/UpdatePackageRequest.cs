using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Packages;

public class UpdatePackageRequest : IRequest<int>
{
    public int Id { get; set; }
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
    public List<PackageItemDto>? PackageItems { get; set; } = new();

}

public class UpdatePackageRequestValidator : CustomValidator<UpdatePackageRequest>
{
    public UpdatePackageRequestValidator(IReadRepository<Package> repository, IStringLocalizer<UpdatePackageRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (package, name, ct) =>
                await repository.GetBySpecAsync(new PackageByNameSpec(name, package.MandantId), ct)
                is not Package existingPackage || existingPackage.Id == package.Id)
                .WithMessage((_, name) => string.Format(localizer["packageName.alreadyexists"], name));

        RuleFor(x => x.Kz)
            .NotEmpty()
            .MaximumLength(10)
            .MustAsync(async (package, kz, ct) =>
                await repository.GetBySpecAsync(new PackageByKzSpec(kz, package.MandantId), ct)
                is not Package existingPackage || existingPackage.Id == package.Id)
                .WithMessage((_, kz) => string.Format(localizer["packageKz.alreadyexists"], kz));

        RuleFor(x => x.Description)
            .MaximumLength(200);
        RuleFor(x => x.Display)
            .MaximumLength(50);
    }
}

public class UpdatePackageRequestHandler : IRequestHandler<UpdatePackageRequest, int>
{
    private readonly IRepositoryWithEvents<Package> _repository;
    private readonly IStringLocalizer<UpdatePackageRequestHandler> _localizer;
    private readonly IRepositoryWithEvents<Period> _periodRepository;
    private readonly IRepositoryWithEvents<PackageItem> _packageItemRepository;

    public UpdatePackageRequestHandler(IRepositoryWithEvents<Package> repository, IRepositoryWithEvents<Period> periodRepository, IRepositoryWithEvents<PackageItem> packageItemRepository, IStringLocalizer<UpdatePackageRequestHandler> localizer) =>
        (_repository, _periodRepository, _packageItemRepository, _localizer) = (repository, periodRepository, packageItemRepository, localizer);

    public async Task<int> Handle(UpdatePackageRequest request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = package ?? throw new NotFoundException(string.Format(_localizer["package.notfound"], request.Id));
        package.Update(
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

        package.DomainEvents.Add(EntityUpdatedEvent.WithEntity(package));
        await _repository.UpdateAsync(package, cancellationToken);

        if (request.PackageItems != null && request.PackageItems.Count() > 0)
        {
            foreach (PackageItemDto packageItemDto in request.PackageItems)
            {
                var packageItem = await _packageItemRepository.GetByIdAsync(packageItemDto.Id, cancellationToken);
                if (packageItem != null)
                {
                    // Update PackageItem
                    packageItem.Start = (DateTime)packageItemDto.Start;
                    packageItem.End = packageItemDto.End;
                    packageItem.ItemId = packageItemDto.ItemId;
                    packageItem.Price = packageItemDto.Price;
                    packageItem.Percentage = packageItemDto.Percentage;
                    packageItem.PackageItemCoreValueEnumId = packageItemDto.PackageItemCoreValueEnumId;
                    packageItem.DomainEvents.Add(EntityCreatedEvent.WithEntity(packageItem));
                    await _packageItemRepository.UpdateAsync(packageItem, cancellationToken);
                }
                else
                {
                    // Create PackageItem
                    PackageItem p = new PackageItem();
                    p.MandantId = packageItemDto.MandantId;
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
        }

        return request.Id;
    }
}