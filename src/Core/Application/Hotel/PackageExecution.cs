using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Shop;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Application.Hotel;

[NotMapped]
public class PackageExecution
{
    public PackageDto? packageDto { get; set; }
    public DateTime arrival { get; set; }
    public DateTime departure { get; set; }
    public DateTime dateCurrently { get; set; }
    public decimal priceCatPax { get; set; }
    public decimal priceBreakfast { get; set; }
    public int roomAmount { get; set; }
    public int adults { get; set; }
    public int childs { get; set; }
    public int amountBreakfast { get; set; }
    public int packageAmount { get; set; }

    public bool isValid
    {
        get
        {
            bool value = false;

            switch (packageDto.PackageBookingRhythmEnumId)
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
                // TODO 290 per Appointment
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

    public BookingLine getBookingLine
    {
        get
        {
            BookingLine bl = new BookingLine();

            switch (packageDto.PackageBookingBaseEnumId)
            {
                case 100:
                    // 100 per Room
                    bl.DateBooking = dateCurrently;
                    bl.Name = packageDto != null ? packageDto.InvoiceName : "not set";
                    bl.InvoicePos = packageDto.InvoicePosition;
                    bl.Source = packageDto.Kz;
                    // bl.includedIn
                    foreach (var item in packageDto.PackageItems)
                    {
                        bl.ItemId = item.ItemId; // package.paket.WarenID;
                                                 //bl.Warennummer = paket.Waren.Warennummer;
                                                 //bl.warengruppe = paket.Waren.Warengruppe.WarengruppeName;

                        // bl.ItemName = 
                        bl.Amount = roomAmount;
                        // bl.TaxId = item.TaxId;
                        // bl.TaxSet = 
                        bl.Price = item.Price != 0 ? item.Price : calculatePackageItemFormula(item);

                    }




                    // bl.PaketmechanikID = paket.PaketmechanikID;
                    //if (package.Preis == 0 && !string.IsNullOrEmpty(paket.Formel))
                    //{
                    //    bl.Betrag = auswertungPaketFormel;
                    //}
                    //else
                    //{
                    //    bl.Betrag = paket.Preis;
                    //}
                    break;
                    //case 2:
                    //    // 2 per Adult
                    //    bl.Datum = datum;
                    //    bl.Beschreibung = paket.Waren.WarenName;
                    //    bl.WarenID = paket.WarenID;
                    //    bl.Warennummer = paket.Waren.Warennummer;
                    //    bl.warengruppe = paket.Waren.Warengruppe.WarengruppeName;
                    //    bl.PaketmechanikID = paket.PaketmechanikID;
                    //    bl.RechPos = paket.RechPos;
                    //    bl.Menge = erwachsene;
                    //    bl.Steuersatz = paket.Waren.Steuer.Steuersatz;

                    //    if (paket.Preis == 0 && !string.IsNullOrEmpty(paket.Formel))
                    //    {
                    //        bl.Betrag = auswertungPaketFormel;
                    //    }
                    //    else
                    //    {
                    //        bl.Betrag = paket.Preis;
                    //    }
                    //    break;
                    //case 3:
                    //    // 3 per Adult
                    //    bl.Datum = datum;
                    //    bl.Beschreibung = paket.Waren.WarenName;
                    //    bl.WarenID = paket.WarenID;
                    //    bl.Warennummer = paket.Waren.Warennummer;
                    //    bl.warengruppe = paket.Waren.Warengruppe.WarengruppeName;
                    //    bl.PaketmechanikID = paket.PaketmechanikID;
                    //    bl.RechPos = paket.RechPos;
                    //    bl.Menge = kinder;
                    //    bl.Steuersatz = paket.Waren.Steuer.Steuersatz;

                    //    if (paket.Preis == 0 && !string.IsNullOrEmpty(paket.Formel))
                    //    {
                    //        bl.Betrag = auswertungPaketFormel;
                    //    }
                    //    else
                    //    {
                    //        bl.Betrag = paket.Preis;
                    //    }
                    //    break;
                    //case 4:
                    //    // 4 per Person
                    //    bl.Datum = datum;
                    //    bl.Beschreibung = paket.Waren.WarenName;
                    //    bl.WarenID = paket.WarenID;
                    //    bl.Warennummer = paket.Waren.Warennummer;
                    //    bl.warengruppe = paket.Waren.Warengruppe.WarengruppeName;
                    //    bl.PaketmechanikID = paket.PaketmechanikID;
                    //    bl.RechPos = paket.RechPos;
                    //    bl.Menge = kinder + erwachsene;
                    //    bl.Steuersatz = paket.Waren.Steuer.Steuersatz;

                    //    if (paket.Preis == 0 && !string.IsNullOrEmpty(paket.Formel))
                    //    {
                    //        bl.Betrag = auswertungPaketFormel;
                    //    }
                    //    else
                    //    {
                    //        bl.Betrag = paket.Preis;
                    //    }
                    //    break;
                    //case 5:
                    //    // 5 per Breakfast
                    //    bl.Datum = datum;
                    //    bl.Beschreibung = paket.Waren.WarenName;
                    //    bl.Menge = anzFrühstück;
                    //    bl.Betrag = frstPreis;
                    //    bl.WarenID = paket.WarenID;
                    //    bl.Warennummer = paket.Waren.Warennummer;
                    //    bl.warengruppe = paket.Waren.Warengruppe.WarengruppeName;
                    //    bl.PaketmechanikID = paket.PaketmechanikID;
                    //    bl.RechPos = paket.RechPos;
                    //    bl.Steuersatz = paket.Waren.Steuer.Steuersatz;

                    //    break;
                    //case 6:
                    //    // 6 per Number
                    //    bl.Datum = datum;
                    //    bl.Beschreibung = paket.Waren.WarenName;
                    //    bl.WarenID = paket.WarenID;
                    //    bl.Warennummer = paket.Waren.Warennummer;
                    //    bl.warengruppe = paket.Waren.Warengruppe.WarengruppeName;
                    //    bl.PaketmechanikID = paket.PaketmechanikID;
                    //    bl.RechPos = paket.RechPos;
                    //    bl.Menge = anzahl;
                    //    bl.Steuersatz = paket.Waren.Steuer.Steuersatz;

                    //    if (paket.Preis == 0 && !string.IsNullOrEmpty(paket.Formel))
                    //    {
                    //        bl.Betrag = auswertungPaketFormel;
                    //    }
                    //    else
                    //    {
                    //        bl.Betrag = paket.Preis;
                    //    }
                    //    break;
            }

            return bl;
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
            else if (item.PackageItemCoreValueEnumId == (int)PackageItemCoreValueEnum.OptionalWithPercentage )
            {
                wert = priceCatPax;
            }

            Console.WriteLine("Wert: " + wert);
            result = wert * item.Percentage;
            return result;
    }


}
