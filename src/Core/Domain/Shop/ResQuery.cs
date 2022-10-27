using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Shop;
public class ResQuery
{
    public string? DestinationTown { get; set; }
    public string? DestinationZipCode { get; set; }
    public string? DestinationDecimalCoordinates { get; set; }
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public List<Pax> RoomOccupancy { get; set; } = new List<Pax>();
    public int RoomAmount
    {
        get
        {
            return RoomOccupancy.Count;
        }
    }

    public int BedsTotal
    {
        get
        {
            int counter = 0;
            foreach (var item in RoomOccupancy)
            {
                counter += item.Beds;
            }

            return counter;
        }
    }
}

public class Pax
{
    public int Adult { get; set; } = 1;
    public List<Child>? Children { get; set; } = new List<Child>();
    public int Beds
    {
        get
        {
            int counter = 0;

            if (Children.Count > 0)
            {
                foreach (Child item in Children)
                {
                    _ = item.ExtraBed ? (counter++) : (counter = counter);
                }
            }

            return counter + Adult;
        }
    }
}

public class Child
{
    public int Age { get; set; }
    public bool ExtraBed { get; set; }
}

public class BookingLine
{
    [Required]
    public int Id { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateBooking { get; set; }
    [StringLength(100)]
    public string? Name { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public decimal PriceTotal => Price * Amount;
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
    [StringLength(30)]
    public string? Source { get; set; } //PackageKz = P:Kz
    public int includedIn { get; set; } // Id wo diese Zeile bestandteil ist.
    public int TaxId { get; set; }
    public int InvoicePos { get; set; }
}

public class BookingLineExtended
{
    [DataType(DataType.Date)]
    public DateTime? DateBooking { get; set; }

    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public decimal PriceTotal => Price * Amount;
    public int InvoicePosition { get; set; }

}

public class BookingLineDetail
{
    public int MandantId { get; set; } // ? ReservationId, BookingId, UserId
    [DataType(DataType.Date)]
    public DateTime DateDetail { get; set; }
    [DataType(DataType.Date)]
    public DateTime DateMandant { get; set; } 
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public decimal PriceTotal => Price * Amount;
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    [StringLength(70)]
    public string? ItemName { get; set; }
    [StringLength(30)]
    public string? Source { get; set; } //PackageKz = P:Kz
    public int TaxId { get; set; }
    public decimal TaxSet { get; set; }
    public int PackageId { get; set; }
}