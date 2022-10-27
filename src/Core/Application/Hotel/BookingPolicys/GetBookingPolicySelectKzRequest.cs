using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.BookingPolicys;

public class GetBookingPolicySelectKzRequest : IRequest<List<BookingPolicySelectKzDto>>
{
    public int MandantId { get; set; }
    public GetBookingPolicySelectKzRequest(int mandantId) => MandantId = mandantId;
}

public class BookingPolicySelectKzByMandantIdSpec : Specification<BookingPolicy, BookingPolicySelectKzDto>
{
    public BookingPolicySelectKzByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId)
             .OrderBy(c => c.Priority);
    }
}

public class GetBookingPolicySelectKzRequestHandler : IRequestHandler<GetBookingPolicySelectKzRequest, List<BookingPolicySelectKzDto>>
{
    private readonly IRepository<BookingPolicy> _repository;
    private readonly IStringLocalizer<GetBookingPolicySelectKzRequestHandler> _localizer;

    public GetBookingPolicySelectKzRequestHandler(IRepository<BookingPolicy> repository, IStringLocalizer<GetBookingPolicySelectKzRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<BookingPolicySelectKzDto>> Handle(GetBookingPolicySelectKzRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<BookingPolicy, BookingPolicySelectKzDto>)new BookingPolicySelectKzByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["BookingPolicySelectKz.notfound"], request.MandantId));
}