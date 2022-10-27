using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.BookingPolicys;

public class GetBookingPolicyRequest : IRequest<BookingPolicyDto>
{
    public int Id { get; set; }
    public GetBookingPolicyRequest(int id) => Id = id;
}

public class GetBookingPolicyRequestHandler : IRequestHandler<GetBookingPolicyRequest, BookingPolicyDto>
{
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

public class BookingPolicyByIdSpec : Specification<BookingPolicy, BookingPolicyDto>, ISingleResultSpecification
{
    public BookingPolicyByIdSpec(int id) => Query.Where(x => x.Id == id);
}