using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.BookingPolicys;

public class SearchBookingPolicysRequest : PaginationFilter, IRequest<PaginationResponse<BookingPolicyDto>>
{
}

public class BookingPolicysBySearchRequestSpec : EntitiesByPaginationFilterSpec<BookingPolicy, BookingPolicyDto>
{
    public BookingPolicysBySearchRequestSpec(SearchBookingPolicysRequest request)
        : base(request) =>
        Query
        .OrderBy(p => p.Name, !request.HasOrderBy());
}

public class SearchBookingPolicysRequestHandler : IRequestHandler<SearchBookingPolicysRequest, PaginationResponse<BookingPolicyDto>>
{
    private readonly IReadRepository<BookingPolicy> _repository;

    public SearchBookingPolicysRequestHandler(IReadRepository<BookingPolicy> repository) =>
        _repository = repository;

    public async Task<PaginationResponse<BookingPolicyDto>> Handle(SearchBookingPolicysRequest request, CancellationToken cancellationToken)
    {
        var spec = new BookingPolicysBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<BookingPolicyDto>(list, count, request.PageNumber, request.PageSize);
    }
}
