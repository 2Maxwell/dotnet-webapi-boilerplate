using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;
public class VCat : AuditableEntity<int>, IAggregateRoot
{

    [Required]
    public int MandantId { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public int Amount { get; set; }
    public int Available { get; set; }
    public int Sold { get; set; }
    public int Stay { get; set; }
    public int Blocked { get; set; }
    public int Arrival { get; set; }
    public int BedsInventory { get; set; }
    public int Beds { get; set; }
    public int ExtraBedsInventory { get; set; }
    public int ExtraBeds { get; set; }
    public int Adult { get; set; }
    public int Child { get; set; }
    public int Departure { get; set; }
    public int Breakfast { get; set; }

    // Nach Buchungsart zählen
    public VCat(int mandantId, DateTime date, int categoryId, int amount, int available, int sold, int stay, int blocked, int arrival, int bedsInventory, int beds, int extraBedsInventory, int extraBeds, int adult, int child, int departure, int breakfast)
    {
        MandantId = mandantId;
        Date = date;
        CategoryId = categoryId;
        Amount = amount;
        Available = available;
        Sold = sold;
        Stay = stay;
        Blocked = blocked;
        Arrival = arrival;
        BedsInventory = bedsInventory;
        Beds = beds;
        ExtraBedsInventory = extraBedsInventory;
        ExtraBeds = extraBeds;
        Adult = adult;
        Child = child;
        Departure = departure;
        Breakfast = breakfast;
    }

    public VCat()
    {
    }

    public VCat RollBackByReservation(int sold, int stay, int arrival, int beds, int extraBeds, int adult, int child, int departure, int breakfast)
    {
        Sold -= sold;
        Stay -= stay;
        Arrival -= arrival;
        Beds -= beds;
        ExtraBeds -= extraBeds;
        Adult -= adult;
        Child -= child;
        Departure -= departure;
        Breakfast -= breakfast;
        return this;
    }

    public VCat RollInByReservation(int sold, int stay, int arrival, int beds, int extraBeds, int adult, int child, int departure, int breakfast)
    {
        Sold += sold;
        Stay += stay;
        Arrival += arrival;
        Beds += beds;
        ExtraBeds += extraBeds;
        Adult += adult;
        Child += child;
        Departure += departure;
        Breakfast += breakfast;
        return this;
    }

}
