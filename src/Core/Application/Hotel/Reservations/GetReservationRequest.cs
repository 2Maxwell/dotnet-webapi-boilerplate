using FSH.WebApi.Application.Accounting.Bookings;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceTags;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.General;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using Mapster;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationRequest : IRequest<ReservationDto>
{
    public GetReservationRequest(int id, int mandantId)
    {
        Id = id;
        MandantId = mandantId;
    }

    public int Id { get; set; }
    public int MandantId { get; set; }
}

public class ReservationByIdSpec : Specification<Reservation, ReservationDto>, ISingleResultSpecification
{
    public ReservationByIdSpec(GetReservationRequest request) => Query
        .Where(x => x.MandantId == request.MandantId && x.Id == request.Id)
        .Include(x => x.Booker);
}

public class PriceTagByReservationIdSpec : Specification<PriceTag, PriceTagDto>, ISingleResultSpecification
{
    public PriceTagByReservationIdSpec(int id) => Query
        .Where(x => x.ReservationId == id)
        .Include(x => x.PriceTagDetails);
}

public class GetReservationRequestHandler : IRequestHandler<GetReservationRequest, ReservationDto>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<PriceTag> _repositoryPriceTag;
    private readonly IRepository<PackageExtend> _repositoryPackageExtend;
    private readonly IRepository<Package> _repositoryPackage;
    private readonly IRepository<PackageItem> _repositoryPackageItem;
    private readonly IRepository<Booking> _repositoryBooking;
    private readonly IRepository<Appointment> _repositoryAppointment;
    private readonly IStringLocalizer<GetReservationRequestHandler> _localizer;

    public GetReservationRequestHandler(IRepository<Reservation> repository, IRepository<PriceTag> repositoryPriceTag, IRepository<PackageExtend> repositoryPackageExtend, IRepository<Package> repositoryPackage, IRepository<PackageItem> repositoryPackageItem, IRepository<Booking> repositoryBooking, IRepository<Appointment> repositoryAppointment, IStringLocalizer<GetReservationRequestHandler> localizer)
    {
        _repository = repository;
        _repositoryPriceTag = repositoryPriceTag;
        _repositoryPackageExtend = repositoryPackageExtend;
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryBooking = repositoryBooking;
        _repositoryAppointment = repositoryAppointment;
        _localizer = localizer;
    }

    public async Task<ReservationDto> Handle(GetReservationRequest request, CancellationToken cancellationToken)
    {
        ReservationDto? reservationDto = await _repository.GetBySpecAsync(
(ISpecification<Reservation, ReservationDto>)new ReservationByIdSpec(request), cancellationToken)
            ?? throw new NotFoundException(string.Format(_localizer["Hallo reservation {0} NotFound"], request.Id));

        reservationDto.PriceTagDto = await _repositoryPriceTag.GetBySpecAsync(
             (ISpecification<PriceTag, PriceTagDto>)new PriceTagByReservationIdSpec(request.Id), cancellationToken);

        // PackageExtend mit allen PackageItems und Package und Appointment wenn vorhanden
        GetPackageExtendOptionsByReservationRequest getPackageExtendOptionsByReservationRequest = new GetPackageExtendOptionsByReservationRequest(request.MandantId, request.Id);
        PackageExtendOptionsByReservationSpec getPackageExtendOptionsByReservationSpec = new PackageExtendOptionsByReservationSpec(getPackageExtendOptionsByReservationRequest.MandantId, getPackageExtendOptionsByReservationRequest.ReservationId);
        IStringLocalizer<GetPackageExtendOptionsByReservationRequestHandler> _localizerAppointment = null;

        GetPackageExtendOptionsByReservationRequestHandler getPackageExtendOptionsByReservationRequestHandler = new GetPackageExtendOptionsByReservationRequestHandler(_repositoryPackageExtend, _repositoryPackage, _repositoryPackageItem, _repositoryAppointment, _localizerAppointment);
        reservationDto.PackageExtendOptionDtos = await getPackageExtendOptionsByReservationRequestHandler.Handle(getPackageExtendOptionsByReservationRequest, cancellationToken);

        var bookings = await _repositoryBooking.ListAsync(
    (ISpecification<Booking, BookingDto>)new BookingsDtoByMandantIdReservationIdSpec(request.MandantId, request.Id), cancellationToken);

        var bookingLines = new List<BookingLine>();
        foreach (BookingDto booking in bookings)
        {
            BookingLine bookingLine = new BookingLine();
            bookingLine = booking.Adapt<BookingLine>();
            bookingLine.DateBooking = booking.HotelDate;
            bookingLines.Add(bookingLine);
        }

        reservationDto.BookingLineSummaries = new List<BookingLineSummary>();
        foreach (var bookingLineGroup in bookingLines.Where(x => x.BookingLineNumberId != null).GroupBy(x => x.BookingLineNumberId))
        {
            BookingLineSummary bookingLineSummary = new BookingLineSummary();
            bookingLineSummary.SourceList = bookingLineGroup.ToList();
            reservationDto.BookingLineSummaries.Add(bookingLineSummary);
        }

        foreach (var bookingLineNonGroup in bookingLines.Where(x => x.BookingLineNumberId == null))
        {
            BookingLineSummary bookingLineSummary = new BookingLineSummary();
            List<BookingLine> bookingLineNonGroupList = new List<BookingLine>();
            bookingLineNonGroupList.Add(bookingLineNonGroup);
            bookingLineSummary.SourceList = bookingLineNonGroupList;
            reservationDto.BookingLineSummaries.Add(bookingLineSummary);
        }

        return reservationDto;
    }
}

public class BookingsDtoByMandantIdReservationIdSpec : Specification<Booking, BookingDto>
{
    public BookingsDtoByMandantIdReservationIdSpec(int mandantId, int reservationId)
    {
        Query.Where(x => x.MandantId == mandantId && x.ReservationId == reservationId && x.State < 5);
    }
}