using FSH.WebApi.Application.Accounting.PluGroups;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Merchandises;

public class UpdateMerchandiseRequest : IRequest<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MerchandiseNumber { get; set; }
    public int TaxId { get; set; }
    public decimal Price { get; set; }
    public int MerchandiseGroupId { get; set; }
    public bool Automatic { get; set; } = false;
}

public class UpdateMerchandiseRequestHandler : IRequestHandler<UpdateMerchandiseRequest, int>
{
    private readonly IRepository<Merchandise> _repository;
    private readonly IStringLocalizer<UpdateMerchandiseRequestHandler> _localizer;

    public UpdateMerchandiseRequestHandler(IRepository<Merchandise> repository, IStringLocalizer<UpdateMerchandiseRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateMerchandiseRequest request, CancellationToken cancellationToken)
    {
        var merchandise = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = merchandise ?? throw new NotFoundException(string.Format(_localizer["merchandise.notfound"], request.Id));

        // Remove old image if there is a new image uploaded
        //  if (request.Image != null)
        // {
        //    string? currentProductImagePath = product.ImagePath;
        //    if (!string.IsNullOrEmpty(currentProductImagePath))
        //    {
        //        string root = Directory.GetCurrentDirectory();
        //        string filePath = currentProductImagePath.Replace("{server_url}/", string.Empty);
        //        _file.Remove(Path.Combine(root, filePath));
        //    }
        // }

        // string? productImagePath = request.Image is not null
        //    ? await _file.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken)
        //    : null;

        var updatedMerchandise = merchandise.Update(request.Name, request.MerchandiseNumber, request.TaxId, request.Price, request.MerchandiseGroupId, request.Automatic);

        // Add Domain Events to be raised after the commit
        merchandise.DomainEvents.Add(EntityUpdatedEvent.WithEntity(merchandise));

        await _repository.UpdateAsync(updatedMerchandise, cancellationToken);

        return request.Id;
    }
}

