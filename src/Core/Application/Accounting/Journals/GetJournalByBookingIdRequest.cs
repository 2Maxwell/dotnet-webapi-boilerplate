using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Journals;
public class GetJournalByBookingIdRequest : IRequest<JournalDto>
{
    public GetJournalByBookingIdRequest(int mandantId, int bookingId)
    {
        MandantId = mandantId;
        BookingId = bookingId;
    }

    public int MandantId { get; set; }
    public int BookingId { get; set; }
}

public class GetJournalByBookingIdRequestSpec : Specification<Journal, JournalDto>
{
    public GetJournalByBookingIdRequestSpec(GetJournalByBookingIdRequest request)
    {
        Query
            .Where(x => x.MandantId == request.MandantId
                           && x.BookingId == request.BookingId);
    }
}

public class GetJournalByBookingIdRequestHandler : IRequestHandler<GetJournalByBookingIdRequest, JournalDto>
{
    private readonly IRepository<Journal> _repository;

    public GetJournalByBookingIdRequestHandler(IRepository<Journal> repository)
    {
        _repository = repository;
    }

    public async Task<JournalDto> Handle(GetJournalByBookingIdRequest request, CancellationToken cancellationToken)
    {
        var spec = new GetJournalByBookingIdRequestSpec(request);
        var result = await _repository.GetBySpecAsync(spec, cancellationToken);
        return result!;
    }
}
