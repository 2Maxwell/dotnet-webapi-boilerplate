using FSH.WebApi.Application.Environment.Persons;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.VipStates;
public class UpdateVipStatusRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Arrival { get; set; }
    public string? Daily { get; set; }
    public string? Departure { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}

public class UpdateVipStatusRequestValidator : CustomValidator<UpdateVipStatusRequest>
{
    public UpdateVipStatusRequestValidator(IReadRepository<VipStatus> repository, IStringLocalizer<UpdateVipStatusRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Kz)
            .MaximumLength(10);
        RuleFor(x => x.Arrival)
            .MaximumLength(150);
        RuleFor(x => x.Daily)
            .MaximumLength(150);
        RuleFor(x => x.Departure)
            .MaximumLength(150);
        RuleFor(x => x.ChipIcon)
            .MaximumLength(100);
        RuleFor(x => x.ChipText)
            .MaximumLength(50);
    }
}

public class UpdateVipStatusRequestHandler : IRequestHandler<UpdateVipStatusRequest, int>
{
    private readonly IRepositoryWithEvents<VipStatus> _repository;
    private readonly IStringLocalizer<UpdateVipStatusRequestHandler> _localizer;

    public UpdateVipStatusRequestHandler(IRepositoryWithEvents<VipStatus> repository, IStringLocalizer<UpdateVipStatusRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateVipStatusRequest request, CancellationToken cancellationToken)
    {
        var vipStatus = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = vipStatus ?? throw new NotFoundException(string.Format(_localizer["vipStatus.notfound"], request.Id));
        vipStatus.Update(
            request.Name,
            request.Kz,
            request.Arrival,
            request.Daily,
            request.Departure,
            request.ChipIcon,
            request.ChipText
            );
        vipStatus.DomainEvents.Add(EntityUpdatedEvent.WithEntity(vipStatus));
        await _repository.UpdateAsync(vipStatus, cancellationToken);
        return request.Id;
    }

}