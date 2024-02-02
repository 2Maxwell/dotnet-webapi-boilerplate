using FSH.WebApi.Application.Accounting;
using FSH.WebApi.Application.Accounting.Bookings;
using FSH.WebApi.Application.Accounting.Items;
using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Application.Accounting.Taxes;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.General;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using Mapster;
using System.Text.Json;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationNightAuditBookingRequest : IRequest<List<BookingLine>>
{
    public GetReservationNightAuditBookingRequest(DateTime? date, ReservationDto reservationDto)
    {
        Date = date;
        this.reservationDto = reservationDto;
    }

    public DateTime? Date { get; set; }
    public ReservationDto reservationDto { get; set; }
}

public class GetReservationNightAuditBookingRequestHandler : IRequestHandler<GetReservationNightAuditBookingRequest, List<BookingLine>>
{
    // private readonly IRepository<Reservation> _repository;
    // private readonly IRepository<PriceTag> _repositoryPriceTag;
    // private readonly IRepository<PackageExtend> _repositoryPackageExtend;
    // private readonly IRepository<Package> _repositoryPackage;
    // private readonly IRepository<PackageItem> _repositoryPackageItem;
    // private readonly IRepository<Booking> _repositoryBooking;
    private readonly IRepository<MandantSetting> _repositoryMandantSetting;

    // private readonly IRepository<Appointment> _repositoryAppointment;
    private readonly IReadRepository<Tax> _repositoryTax;
    private readonly IRepository<TaxItem> _repositoryTaxItem;
    private readonly IReadRepository<Item> _repositoryItem;
    private readonly IRepository<ItemPriceTax> _repositoryItemPriceTax;
    private readonly IRepository<Package> _repositoryPackage;
    private readonly IRepository<PackageItem> _repositoryPackageItem;
    private readonly IRepository<Booking> _repositoryBooking;
    private readonly IRepository<Journal> _repositoryJournal;
    private readonly IRepository<MandantNumbers> _repositoryMandantNumbers;
    private readonly IRepository<CashierJournal> _repositoryCashierJournal;

    public GetReservationNightAuditBookingRequestHandler(IRepository<MandantSetting> repositoryMandantSetting, IReadRepository<Tax> repositoryTax, IRepository<TaxItem> repositoryTaxItem, IReadRepository<Item> repositoryItem, IRepository<ItemPriceTax> repositoryItemPriceTax, IRepository<Package> repositoryPackage, IRepository<PackageItem> repositoryPackageItem, IRepository<Booking> repositoryBooking, IRepository<Journal> repositoryJournal, IRepository<MandantNumbers> repositoryMandantNumbers, IRepository<CashierJournal> repositoryCashierJournal)
    {
        _repositoryMandantSetting = repositoryMandantSetting;
        _repositoryTax = repositoryTax;
        _repositoryTaxItem = repositoryTaxItem;
        _repositoryItem = repositoryItem;
        _repositoryItemPriceTax = repositoryItemPriceTax;
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryBooking = repositoryBooking;
        _repositoryJournal = repositoryJournal;
        _repositoryMandantNumbers = repositoryMandantNumbers;
        _repositoryCashierJournal = repositoryCashierJournal;
    }


    // private readonly IStringLocalizer<GetReservationNightAuditBookingRequestHandler> _localizer;



    public async Task<List<BookingLine>> Handle(GetReservationNightAuditBookingRequest request, CancellationToken cancellationToken)
    {
        List<BookingLine> bookingLines = new List<BookingLine>();
        Pax? pax = JsonSerializer.Deserialize<Pax>(request.reservationDto!.PaxString!);

        var mandantSettingSpec = new GetMandantSettingByMandantIdSpec(request.reservationDto.MandantId);
        MandantSettingDto? mandantSettingDto = await _repositoryMandantSetting.GetBySpecAsync((ISpecification<MandantSetting, MandantSettingDto>)mandantSettingSpec, cancellationToken);

        var taxesSpec = new TaxesByCountryIdSpec(mandantSettingDto!.TaxCountryId);
        List<TaxDto>? taxesList = await _repositoryTax.ListAsync(taxesSpec, cancellationToken);

        // In taxesList für jedes TaxDto die TaxItems laden
        foreach (var item in taxesList)
        {
            item.TaxItems = new List<TaxItemDto>();
            var taxItems = await _repositoryTaxItem.ListAsync((ISpecification<TaxItem, TaxItemDto>)new TaxItemByTaxIdSpec(item.Id), cancellationToken);
                           // ?? throw new NotFoundException(string.Format(_localizer["TaxItems.notfound"], item.Id));
            item.TaxItems = taxItems;
        }

        var itemsSpec = new ItemsByMandantIdAndMandantId0Spec(request.reservationDto.MandantId);
        List<ItemDto> itemsList = await _repositoryItem.ListAsync(itemsSpec, cancellationToken);

        foreach (var item in itemsList)
        {
            item.PriceTaxesDto = await _repositoryItemPriceTax.ListAsync((ISpecification<ItemPriceTax, ItemPriceTaxDto>)new ItemPriceTaxByItemIdSpec(item.Id), cancellationToken);
        }

        List<PackageDto>? packageList = new List<PackageDto>();

        if (request.reservationDto.RatePackages != null || request.reservationDto.RatePackages != string.Empty)
        {
            string[] packagesKz = request.reservationDto!.RatePackages!.Split(',', StringSplitOptions.TrimEntries);

            for (int i = 0; i < packagesKz.Length; i++)
            {
                if (packagesKz[i] != string.Empty)
                {
                    // lade PackageDto
                    var spec = new PackageDtoByKzSpec(packagesKz[i], request.reservationDto.MandantId);
                    PackageDto? pack = await _repositoryPackage.GetBySpecAsync((ISpecification<Package, PackageDto>)spec, cancellationToken);

                    // lade PackageItemDto
                    List<PackageItemDto> listPackageItems = new List<PackageItemDto>();
                    pack!.PackageItems = await _repositoryPackageItem.ListAsync((ISpecification<PackageItem, PackageItemDto>)new PackageItemByPackageIdSpec(pack.Id), cancellationToken);

                    // in Liste der Packages in der Rate einfügen
                    packageList.Add(pack);
                }
            }
        }

        foreach (PackageDto pack in packageList)
        {
            PackageExtendDto packageExtendDto = new PackageExtendDto();
            packageExtendDto.PackageDto = pack;

            PackageExecution packageExecution = new PackageExecution();
            packageExecution.packageExtendedDto = packageExtendDto;
            packageExecution.arrival = Convert.ToDateTime(request.reservationDto.Arrival);
            packageExecution.departure = Convert.ToDateTime(request.reservationDto.Departure);
            packageExecution.dateCurrently = Convert.ToDateTime(request.Date).Date;

            decimal priceLogis = 0;
            if (request.reservationDto!.PriceTagDto!.RateSelected == 1) priceLogis = request.reservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == Convert.ToDateTime(request.Date).Date).Select(x => x.RateCurrent).FirstOrDefault();
            if (request.reservationDto!.PriceTagDto!.RateSelected == 2) priceLogis = request.reservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == Convert.ToDateTime(request.Date).Date).Select(x => x.AverageRate).FirstOrDefault();
            if (request.reservationDto!.PriceTagDto!.RateSelected == 3) priceLogis = request.reservationDto!.PriceTagDto!.UserRate!.Value; // request.ReservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == i.ToDateTime(TimeOnly.Parse("0:00 PM")).Date).Select(x => x.UserRate!.Value).FirstOrDefault();
            if (priceLogis == 0 & request.reservationDto!.PriceTagDto!.RateSelected != 3) priceLogis = request.reservationDto!.PriceTagDto!.AverageRate;


            packageExecution.priceCatPax = priceLogis; // request.ReservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == i.Date).Select(x => x.RateCurrent).FirstOrDefault();
            packageExecution.priceBreakfast = 99;
            packageExecution.roomAmount = request.reservationDto.RoomAmount;
            packageExecution.adults = pax!.Adult;
            packageExecution.children = pax!.Children!;
            packageExecution.amountBreakfast = 1;
            packageExecution.packageAmount = 0;
            packageExecution.bookingLineNumber = packageExtendDto.PackageDto.Kz + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            packageExecution.itemsList = itemsList;
            packageExecution.taxesList = taxesList;

            if (packageExecution.isValid)
            {
                bookingLines.AddRange(packageExecution.getBookingLines);

            }
        }

        foreach (PackageExtendDto packExt in request.reservationDto.PackageExtendOptionDtos!)
        {

            PackageExecution packageExecution = new PackageExecution();
            packageExecution.packageExtendedDto = packExt;
            packageExecution.arrival = Convert.ToDateTime(request.reservationDto.Arrival);
            packageExecution.departure = Convert.ToDateTime(request.reservationDto.Departure);
            packageExecution.dateCurrently = Convert.ToDateTime(request.Date).Date;
            packageExecution.priceCatPax = request.reservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == Convert.ToDateTime(request.Date).Date).Select(x => x.RateCurrent).FirstOrDefault();
            packageExecution.priceBreakfast = 99;
            packageExecution.roomAmount = request.reservationDto.RoomAmount;
            packageExecution.adults = pax!.Adult;
            packageExecution.children = pax!.Children!;
            packageExecution.amountBreakfast = 1;
            packageExecution.packageAmount = packExt.Amount;
            packageExecution.bookingLineNumber = packExt.PackageDto.Kz + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            packageExecution.itemsList = itemsList;
            packageExecution.taxesList = taxesList;
            

            if (packageExecution.isValid)
            {
                bookingLines.AddRange(packageExecution.getBookingLines);
            }
        }

        // Nimm die BookingLines und füge sie in Bookings ein mit CreateBookingBulkRequest
        List<CreateBookingRequest> createBookings = new List<CreateBookingRequest>();
        foreach (var bookingLine in bookingLines)
        {
            CreateBookingRequest createBookingRequest = new CreateBookingRequest();
            createBookingRequest.MandantId = request.reservationDto.MandantId;
            createBookingRequest.HotelDate = Convert.ToDateTime(request.Date);
            createBookingRequest.ReservationId = request.reservationDto.Id;
            createBookingRequest.Name = bookingLine.Name;
            createBookingRequest.Amount = bookingLine.Amount;
            createBookingRequest.Price = bookingLine.Price;
            createBookingRequest.Debit = bookingLine.Debit;
            createBookingRequest.ItemId = bookingLine.ItemId;
            createBookingRequest.ItemNumber = bookingLine.ItemNumber;
            createBookingRequest.Source = $"NightAudit ResId: #{request.reservationDto.Id}";
            createBookingRequest.BookingLineNumberId = bookingLine.BookingLineNumberId;
            createBookingRequest.TaxId = bookingLine.TaxId;
            createBookingRequest.TaxRate = bookingLine.TaxRate;
            createBookingRequest.InvoicePos = bookingLine.InvoicePos;
            createBookings.Add(createBookingRequest);
        }

        CreateBookingBulkRequest createBookingBulkRequest = new CreateBookingBulkRequest(createBookings, request.reservationDto.MandantId);
        CreateBookingBulkRequestHandler createBookingBulkRequestHandler = new CreateBookingBulkRequestHandler(_repositoryBooking, _repositoryJournal, _repositoryMandantNumbers, _repositoryCashierJournal);
        bool result = await createBookingBulkRequestHandler.Handle(createBookingBulkRequest, cancellationToken);

        // Buchungen für die Reservierung
        // var bookingList = await _repositoryBooking.ListAsync(new BookingByReservationIdSpec(request.reservationDto.Id), cancellationToken);
        // foreach (var booking in bookingList)
        // {
        //    BookingLine bookingLine = new BookingLine();
        //    bookingLine.BookingId = booking.Id;
        //    bookingLine.BookingDate = booking.BookingDate;
        //    bookingLine.BookingType = booking.BookingType;
        //    bookingLine.BookingTypeId = booking.BookingTypeId;
        //    bookingLine.BookingTypeText = booking.BookingTypeText;
        //    bookingLine.BookingText = booking.BookingText;
        //    bookingLine.BookingTextId = booking.BookingTextId;
        //    bookingLine.BookingTextText = booking.BookingTextText;
        //    bookingLine.BookingAmount = booking.BookingAmount;
        //    bookingLine.BookingAmountNet = booking.BookingAmountNet;
        //    bookingLine
        // }

        return bookingLines;
    }
}
