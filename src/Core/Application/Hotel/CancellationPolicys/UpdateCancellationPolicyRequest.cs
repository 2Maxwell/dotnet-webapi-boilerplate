using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class UpdateCancellationPolicyRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? Display { get; set; }
    public string? DisplayShort { get; set; }
    public string? DisplayHighLight { get; set; }
    public string? ConfirmationText { get; set; }
    public bool IsDefault { get; set; }
    public int FreeCancellationDays { get; set; }
    public int Priority { get; set; }
    public int NoShow { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}

public class GetBookingPolicyRequestHandler : IRequestHandler<GetBookingPolicyRequest, BookingPolicyDto>
{

    // private readonly HttpClient _httpClient;
    private readonly IRepository<BookingPolicy> _repository;
    private readonly IStringLocalizer<GetBookingPolicyRequestHandler> _localizer;

    public GetBookingPolicyRequestHandler(IRepository<BookingPolicy> repository, IStringLocalizer<GetBookingPolicyRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<BookingPolicyDto> Handle(GetBookingPolicyRequest request, CancellationToken cancellationToken)
    {
        BookingPolicyDto? bookingPolicyDto = await _repository.GetBySpecAsync(
            (ISpecification<BookingPolicy, BookingPolicyDto>)new BookingPolicyByIdSpec(request.Id), cancellationToken);

        // ?? throw new NotFoundException(string.Format(_localizer["bookingPolicy.notfound"], request.Id));

        if (bookingPolicyDto == null) throw new NotFoundException(string.Format(_localizer["bookingPolicy.notfound"], request.Id));

        return bookingPolicyDto;
    }
}

public class UpdateCancellationPolicyRequestHandler : IRequestHandler<UpdateCancellationPolicyRequest, int>
{
    private readonly IRepositoryWithEvents<CancellationPolicy> _repository;
    private readonly IStringLocalizer<UpdateCancellationPolicyRequestHandler> _localizer;

    public UpdateCancellationPolicyRequestHandler(IRepositoryWithEvents<CancellationPolicy> repository, IStringLocalizer<UpdateCancellationPolicyRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateCancellationPolicyRequest request, CancellationToken cancellationToken)
    {
        var cancellationPolicy = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = cancellationPolicy ?? throw new NotFoundException(string.Format(_localizer["cancellationPolicy.notfound"], request.Id));
        cancellationPolicy.Update(
            request.Name,
            request.Kz,
            request.Description,
            request.Display,
            request.DisplayShort,
            request.DisplayHighLight,
            request.ConfirmationText,
            request.IsDefault,
            request.FreeCancellationDays,
            request.Priority,
            request.NoShow,
            request.ChipIcon,
            request.ChipText
            );

        cancellationPolicy.DomainEvents.Add(EntityUpdatedEvent.WithEntity(cancellationPolicy));
        await _repository.UpdateAsync(cancellationPolicy, cancellationToken);

        return request.Id;
    }
}

