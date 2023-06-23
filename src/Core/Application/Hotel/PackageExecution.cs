using FSH.WebApi.Application.Accounting.Items;
using FSH.WebApi.Application.Accounting.Taxes;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Shop;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Application.Hotel;

[NotMapped]
public class PackageExecution
{
    public PackageExtendDto? packageExtendedDto { get; set; }
    public DateTime arrival { get; set; }
    public DateTime departure { get; set; }
    public DateTime dateCurrently { get; set; }
    public decimal priceCatPax { get; set; }
    public decimal priceBreakfast { get; set; }
    public int roomAmount { get; set; }
    public int adults { get; set; }
    // public int childs { get; set; }
    public List<Child>? children { get; set; }
    public int amountBreakfast { get; set; }
    public decimal packageAmount { get; set; }
    public int bookingLineNumber { get; set; }
    public List<ItemDto>? itemsList { get; set; }
    public List<TaxDto>? taxesList { get; set; }

    public bool isValid
    {
        get
        {
            bool value = false;

            switch (packageExtendedDto.PackageDto.PackageBookingRhythmEnumId)
            {
                case 100:
                    // 100 = daily
                    if (arrival.Date <= dateCurrently.Date) value = true;
                    break;
                // TODO perHour, per45Minutes, per30Minutes, per15Minutes, perMinute
                case 200:
                    // 200 = on Arrival
                    if (arrival.Date == dateCurrently.Date) value = true;
                    break;
                case 210:
                    // 210 = on Departure
                    if (departure.Date == dateCurrently.AddDays(1).Date) value = true;
                    break;
                case 290:
                    // 290 per Appointment
                    // wenn noch nicht Status = Booked dann wenn Departure morgen ist dann buchen.
                    if (packageExtendedDto!.PackageExtendedStateEnum == PackageExtendedStateEnum.cartItem) value = true;
                    else if (packageExtendedDto.PackageExtendedStateEnum == PackageExtendedStateEnum.pending && departure.Date == dateCurrently.AddDays(1)) value = true;
                    else if (packageExtendedDto.PackageExtendedStateEnum == PackageExtendedStateEnum.reserved && Convert.ToDateTime(packageExtendedDto.Appointment).Date == dateCurrently.Date) value = true;
                    break;
                case 291:
                    // TODO 291 NumberPrice
                    // Am AnreiseTag buchen.
                    if (arrival.Date == dateCurrently.Date) value = true;
                    break;
                case 300:
                    // 300 = Monday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Monday) value = true;
                    break;
                case 310:
                    // 310 = Tuesday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Tuesday) value = true;
                    break;
                case 320:
                    // 320 = Wednesday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Wednesday) value = true;
                    break;
                case 330:
                    // 330 = Thursday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Thursday) value = true;
                    break;
                case 340:
                    // 340 = Friday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Friday) value = true;
                    break;
                case 350:
                    // 350 = Saturday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Saturday) value = true;
                    break;
                case 360:
                    // 360 = Sunday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Sunday) value = true;
                    break;
                case 400:
                    // 400 = Monday - Thursday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Monday | dateCurrently.DayOfWeek == DayOfWeek.Tuesday | dateCurrently.DayOfWeek == DayOfWeek.Wednesday | dateCurrently.DayOfWeek == DayOfWeek.Thursday) value = true;
                    break;
                case 410:
                    // 410 = Monday - Friday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Monday | dateCurrently.DayOfWeek == DayOfWeek.Tuesday | dateCurrently.DayOfWeek == DayOfWeek.Wednesday | dateCurrently.DayOfWeek == DayOfWeek.Thursday | dateCurrently.DayOfWeek == DayOfWeek.Friday) value = true;
                    break;
                case 420:
                    // 420 = Friday - Sunday
                    if (dateCurrently.DayOfWeek == DayOfWeek.Friday | dateCurrently.DayOfWeek == DayOfWeek.Saturday | dateCurrently.DayOfWeek == DayOfWeek.Sunday) value = true;
                    break;

                case 430:
                    // 430 = Saturday - Sunday
                    if (dateCurrently.DayOfWeek >= DayOfWeek.Saturday & dateCurrently.DayOfWeek <= DayOfWeek.Sunday) value = true;
                    break;
                case 500:
                    // 500 = 2. Day
                    if (arrival.Date == dateCurrently.AddDays(-1).Date) value = true;
                    break;
                case 510:
                    // 510 = 3. Day
                    if (arrival.Date == dateCurrently.AddDays(-2).Date) value = true;
                    break;
                case 520:
                    // 520 = 4. Day
                    if (arrival.Date == dateCurrently.AddDays(-3).Date) value = true;
                    break;
                case 530:
                    // 530 = 5. Day
                    if (arrival.Date == dateCurrently.AddDays(-4).Date) value = true;
                    break;
                case 540:
                    // 540 = 6. Day
                    if (arrival.Date == dateCurrently.AddDays(-5).Date) value = true;
                    break;
                case 550:
                    // 550 = 7. Day
                    if (arrival.Date == dateCurrently.AddDays(-6).Date) value = true;
                    break;
            }

            return value;
        }
    }

    public List<BookingLine> getBookingLines
    {
        get
        {
            List<BookingLine> list = new List<BookingLine>();

            switch (packageExtendedDto.PackageDto.PackageBookingBaseEnumId)
            {
                case 100:
                    // 100 per Room
                    foreach (var item in packageExtendedDto.PackageDto.PackageItems)
                    {
                        BookingLine bl = new BookingLine();
                        bl.DateBooking = dateCurrently;
                        bl.Name = packageExtendedDto != null ? packageExtendedDto.PackageDto.InvoiceName : "not set";
                        bl.InvoicePos = packageExtendedDto.PackageDto.InvoicePosition;
                        bl.Source = packageExtendedDto.PackageDto.Id + "|" + packageExtendedDto.PackageDto.Kz;
                        // bl.includedIn
                        ItemDto? itDto = itemsList != null ? itemsList.Where(x => x.Id == item.ItemId).FirstOrDefault() : null;

                        bl.ItemId = item.ItemId; // package.paket.WarenID;
                        // bl.ItemNumber = ItemTaxList.Where(x => x.Id == item.ItemId).Select(x => x.ItemNumber).FirstOrDefault();
                        bl.ItemNumber = itDto != null ? itDto.ItemNumber : 0;
                        // bl.ItemName = ItemTaxList.Where(x => x.Id == item.ItemId).Select(x => x.Name).FirstOrDefault();
                        bl.ItemName = itDto.Name;
                        bl.Amount = roomAmount;

                        // bl.TaxId = item.TaxId;
                        // Anhand von dateCurrently in itDto.PriceTaxes die gültige TaxId ermitteln
                        bl.TaxId = itDto.PriceTaxesDto.Where(ti => ti.Start <= dateCurrently && ti.End >= dateCurrently).Select(ti => ti.TaxId).FirstOrDefault();
                        // bl.TaxId = itDto.TaxId;  //itDto.Taxes.FirstOrDefault().TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.Id).FirstOrDefault();
                        TaxDto? taxDto = taxesList.Where(x => x.Id == bl.TaxId).FirstOrDefault();
                        bl.TaxRate = taxDto.TaxItems.Where(ti => ti.Start <= dateCurrently && ti.End >= dateCurrently).Select(ti => ti.TaxRate).FirstOrDefault();

                        bl.Price = item.Price != 0 ? item.Price : calculatePackageItemFormula(item);
                        bl.BookingLineNumberId = bookingLineNumber;
                        list.Add(bl);
                    }

                    break;

                case 200:
                    // 200 per Adult
                    foreach (var item in packageExtendedDto.PackageDto.PackageItems.Where(x => x.Start <= dateCurrently && x.End >= dateCurrently))
                    {
                        if (item.ItemId != 0)
                        {
                        BookingLine bl = new BookingLine();
                        bl.DateBooking = dateCurrently;
                        bl.Name = packageExtendedDto != null ? packageExtendedDto.PackageDto.InvoiceName : "not set";
                        bl.InvoicePos = packageExtendedDto.PackageDto.InvoicePosition;
                        bl.Source = packageExtendedDto.PackageDto.Id + "|" + packageExtendedDto.PackageDto.Kz;
                        ItemDto? itDto = itemsList != null ? itemsList.Where(x => x.Id == item.ItemId).FirstOrDefault() : null;
                        bl.ItemId = item.ItemId; // package.paket.WarenID;
                        bl.ItemNumber = itDto != null ? itDto.ItemNumber : 0;
                        bl.ItemName = itDto.Name;
                        bl.Amount = adults;

                            // bl.TaxId = item.TaxId;
                            // Anhand von dateCurrently in itDto.PriceTaxes die gültige TaxId ermitteln
                            bl.TaxId = itDto.PriceTaxesDto.Where(ti => ti.Start <= dateCurrently && ti.End >= dateCurrently).Select(ti => ti.TaxId).FirstOrDefault();
                            // bl.TaxId = itDto.TaxId;  //itDto.Taxes.FirstOrDefault().TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.Id).FirstOrDefault();
                            TaxDto? taxDto = taxesList.Where(x => x.Id == bl.TaxId).FirstOrDefault();
                        bl.TaxRate = taxDto.TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.TaxRate).FirstOrDefault();
                        bl.Price = item.Price != 0 ? item.Price : calculatePackageItemFormula(item);
                        bl.BookingLineNumberId = bookingLineNumber;
                        list.Add(bl);

                        }

                    }

                    break;

                case 300:
                    // 300 per Child
                    foreach (var item in packageExtendedDto.PackageDto.PackageItems)
                    {
                        BookingLine bl = new BookingLine();
                        bl.DateBooking = dateCurrently;
                        bl.Name = packageExtendedDto != null ? packageExtendedDto.PackageDto.InvoiceName : "not set";
                        bl.InvoicePos = packageExtendedDto.PackageDto.InvoicePosition;
                        ItemDto? itDto = itemsList != null ? itemsList.Where(x => x.Id == item.ItemId).FirstOrDefault() : null;
                        bl.Source = packageExtendedDto.PackageDto.Id + "|" + packageExtendedDto.PackageDto.Kz;
                        bl.ItemId = item.ItemId;
                        bl.ItemNumber = itDto != null ? itDto.ItemNumber : 0;
                        bl.ItemName = itDto.Name;
                        bl.Amount = children.Count;

                        // bl.TaxId = item.TaxId;
                        // Anhand von dateCurrently in itDto.PriceTaxes die gültige TaxId ermitteln
                        bl.TaxId = itDto.PriceTaxesDto.Where(ti => ti.Start <= dateCurrently && ti.End >= dateCurrently).Select(ti => ti.TaxId).FirstOrDefault();
                        // bl.TaxId = itDto.TaxId;  //itDto.Taxes.FirstOrDefault().TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.Id).FirstOrDefault();
                        TaxDto? taxDto = taxesList.Where(x => x.Id == bl.TaxId).FirstOrDefault();
                        bl.TaxRate = taxDto.TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.TaxRate).FirstOrDefault();
                        bl.Price = item.Price != 0 ? item.Price : calculatePackageItemFormula(item);
                        bl.BookingLineNumberId = bookingLineNumber;
                        list.Add(bl);
                    }

                    break;

                case 310:
                    // 310 per Child5_15
                    foreach (var item in packageExtendedDto.PackageDto.PackageItems)
                    {
                        BookingLine bl = new BookingLine();
                        bl.DateBooking = dateCurrently;
                        bl.Name = packageExtendedDto != null ? packageExtendedDto.PackageDto.InvoiceName : "not set";
                        bl.InvoicePos = packageExtendedDto.PackageDto.InvoicePosition;
                        ItemDto? itDto = itemsList != null ? itemsList.Where(x => x.Id == item.ItemId).FirstOrDefault() : null;
                        bl.Source = packageExtendedDto.PackageDto.Id + "|" + packageExtendedDto.PackageDto.Kz;
                        bl.ItemId = item.ItemId;
                        bl.ItemNumber = itDto != null ? itDto.ItemNumber : 0;
                        bl.ItemName = itDto.Name;
                        bl.Amount = children.Where(x => x.Age >= 5 && x.Age <= 15).Count();

                        // bl.TaxId = item.TaxId;
                        // Anhand von dateCurrently in itDto.PriceTaxes die gültige TaxId ermitteln
                        bl.TaxId = itDto.PriceTaxesDto.Where(ti => ti.Start <= dateCurrently && ti.End >= dateCurrently).Select(ti => ti.TaxId).FirstOrDefault();
                        // bl.TaxId = itDto.TaxId;  //itDto.Taxes.FirstOrDefault().TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.Id).FirstOrDefault();
                        TaxDto? taxDto = taxesList.Where(x => x.Id == bl.TaxId).FirstOrDefault();
                        bl.TaxRate = taxDto.TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.TaxRate).FirstOrDefault();
                        bl.Price = item.Price != 0 ? item.Price : calculatePackageItemFormula(item);
                        bl.BookingLineNumberId = bookingLineNumber;
                        list.Add(bl);
                    }

                    break;

                case 400:
                    // 400 per Person
                    foreach (var item in packageExtendedDto.PackageDto.PackageItems)
                    {
                        BookingLine bl = new BookingLine();
                        bl.DateBooking = dateCurrently;
                        bl.Name = packageExtendedDto != null ? packageExtendedDto.PackageDto.InvoiceName : "not set";
                        bl.InvoicePos = packageExtendedDto.PackageDto.InvoicePosition;
                        ItemDto? itDto = itemsList != null ? itemsList.Where(x => x.Id == item.ItemId).FirstOrDefault() : null;
                        bl.Source = packageExtendedDto.PackageDto.Id + "|" + packageExtendedDto.PackageDto.Kz;
                        bl.ItemId = item.ItemId;
                        bl.ItemNumber = itDto != null ? itDto.ItemNumber : 0;
                        bl.ItemName = itDto.Name;
                        bl.Amount = adults + children.Count;

                        // bl.TaxId = item.TaxId;
                        // Anhand von dateCurrently in itDto.PriceTaxes die gültige TaxId ermitteln
                        bl.TaxId = itDto.PriceTaxesDto.Where(ti => ti.Start <= dateCurrently && ti.End >= dateCurrently).Select(ti => ti.TaxId).FirstOrDefault();
                        // bl.TaxId = itDto.TaxId;  //itDto.Taxes.FirstOrDefault().TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.Id).FirstOrDefault();
                        TaxDto? taxDto = taxesList.Where(x => x.Id == bl.TaxId).FirstOrDefault();
                        bl.TaxRate = taxDto.TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.TaxRate).FirstOrDefault();
                        bl.Price = item.Price != 0 ? item.Price : calculatePackageItemFormula(item);
                        bl.BookingLineNumberId = bookingLineNumber;
                        list.Add(bl);
                    }

                    break;

                case 500:
                    // 500 per Breakfast
                    foreach (var item in packageExtendedDto.PackageDto.PackageItems)
                    {
                        BookingLine bl = new BookingLine();
                        bl.DateBooking = dateCurrently;
                        bl.Name = packageExtendedDto != null ? packageExtendedDto.PackageDto.InvoiceName : "not set";
                        bl.InvoicePos = packageExtendedDto.PackageDto.InvoicePosition;
                        ItemDto? itDto = itemsList != null ? itemsList.Where(x => x.Id == item.ItemId).FirstOrDefault() : null;
                        bl.Source = packageExtendedDto.PackageDto.Id + "|" + packageExtendedDto.PackageDto.Kz;
                        bl.ItemId = item.ItemId;
                        bl.ItemNumber = itDto != null ? itDto.ItemNumber : 0;
                        bl.ItemName = itDto.Name;
                        bl.Amount = amountBreakfast;

                        // bl.TaxId = item.TaxId;
                        // Anhand von dateCurrently in itDto.PriceTaxes die gültige TaxId ermitteln
                        bl.TaxId = itDto.PriceTaxesDto.Where(ti => ti.Start <= dateCurrently && ti.End >= dateCurrently).Select(ti => ti.TaxId).FirstOrDefault();
                        // bl.TaxId = itDto.TaxId;  //itDto.Taxes.FirstOrDefault().TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.Id).FirstOrDefault();
                        TaxDto? taxDto = taxesList.Where(x => x.Id == bl.TaxId).FirstOrDefault();
                        bl.TaxRate = taxDto.TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.TaxRate).FirstOrDefault();
                        bl.Price = item.Price != 0 ? item.Price : calculatePackageItemFormula(item);
                        bl.BookingLineNumberId = bookingLineNumber;
                        list.Add(bl);
                    }

                    break;

                case 600:
                    // 600 per Number
                    foreach (var item in packageExtendedDto.PackageDto.PackageItems.Where(x => x.Start <= dateCurrently && x.End >= dateCurrently))
                    {
                        if (item.ItemId != 0)
                        {
                            BookingLine bl = new BookingLine();
                            bl.DateBooking = dateCurrently;
                            bl.Name = packageExtendedDto != null ? packageExtendedDto.PackageDto.InvoiceName : "not set";
                            bl.InvoicePos = packageExtendedDto.PackageDto.InvoicePosition;
                            ItemDto? itDto = itemsList != null ? itemsList.Where(x => x.Id == item.ItemId).FirstOrDefault() : null;
                            bl.Source = packageExtendedDto.PackageDto.Id + "|" + packageExtendedDto.PackageDto.Kz;
                            bl.ItemId = item.ItemId;
                            bl.ItemNumber = itDto != null ? itDto.ItemNumber : 0;
                            bl.ItemName = itDto.Name;
                            bl.Amount = packageAmount * roomAmount;

                            // bl.TaxId = item.TaxId;
                            // Anhand von dateCurrently in itDto.PriceTaxes die gültige TaxId ermitteln
                            bl.TaxId = itDto.PriceTaxesDto.Where(ti => ti.Start <= dateCurrently && ti.End >= dateCurrently).Select(ti => ti.TaxId).FirstOrDefault();
                            // bl.TaxId = itDto.TaxId;  //itDto.Taxes.FirstOrDefault().TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.Id).FirstOrDefault();
                            TaxDto? taxDto = taxesList.Where(x => x.Id == bl.TaxId).FirstOrDefault();
                            bl.TaxRate = taxDto.TaxItems.Where(ti => ti.Start <= arrival && ti.End >= departure).Select(ti => ti.TaxRate).FirstOrDefault();
                            bl.Price = item.Price != 0 ? item.Price : calculatePackageItemFormula(item);
                            bl.BookingLineNumberId = bookingLineNumber;
                            list.Add(bl);
                        }
                    }

                    break;

            }

            return list;
        }
    }

    private decimal calculatePackageItemFormula(PackageItemDto item)
    {
        decimal result = 0;
        decimal wert = 0;

        if (item.PackageItemCoreValueEnumId == (int)PackageItemCoreValueEnum.Roomrate)
        {
            wert = priceCatPax;
        }
        else if (item.PackageItemCoreValueEnumId == (int)PackageItemCoreValueEnum.OptionalWithPercentage)
        {
            wert = priceCatPax;
        }
        else if (item.PackageItemCoreValueEnumId == (int)PackageItemCoreValueEnum.PriceLine)
        {
            wert = packageExtendedDto.PackageDto.PackageItems.Where(x => x.ItemId == 0 && x.Start <= dateCurrently && x.End >= dateCurrently).Select(x => x.Price).FirstOrDefault();
        }

        result = (wert * item.Percentage) / 100;
        return result;
    }

}
