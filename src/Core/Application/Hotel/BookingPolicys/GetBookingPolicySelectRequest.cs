using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.BookingPolicys;

public class GetBookingPolicySelectRequest : IRequest<List<BookingPolicySelectDto>>
{
    public int MandantId { get; set; }
    public GetBookingPolicySelectRequest(int mandantId) => MandantId = mandantId;
}

public class BookingPolicyByMandantIdSpec : Specification<BookingPolicy, BookingPolicySelectDto>
{
    public BookingPolicyByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId)
             .OrderBy(c => c.Priority);
    }
}

public class GetBookingPolicySelectRequestHandler : IRequestHandler<GetBookingPolicySelectRequest, List<BookingPolicySelectDto>>
{
    private readonly IRepository<BookingPolicy> _repository;
    private readonly IStringLocalizer<GetBookingPolicySelectRequestHandler> _localizer;

    public GetBookingPolicySelectRequestHandler(IRepository<BookingPolicy> repository, IStringLocalizer<GetBookingPolicySelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<BookingPolicySelectDto>> Handle(GetBookingPolicySelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<BookingPolicy, BookingPolicySelectDto>)new BookingPolicyByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["BookingPolicySelect.notfound"], request.MandantId));
}

