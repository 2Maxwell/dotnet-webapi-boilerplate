namespace FSH.WebApi.Application.Hotel.Reservations;

public class ReservationStatArrivalDto : IDto
{
    public int MandantId { get; set; }
    public DateTime? Date { get; set; }
    public int? ReservationToday { get; set; } // Count of Reservations with Arrival = Date
    public int? ReservationRoomsToday { get; set; } // Count of Rooms with Arrival = Date
    public int? ReservationAdultToday { get; set; } // Count of Pax with Arrival = Date
    public int? ReservationChildToday { get; set; } // Count of Child with Arrival = Date
    public int? ReservationExtraBedToday { get; set; } // Count of ExtraBed with Arrival = Date

    public int? ReservationTodayDone { get; set; } // Count of Reservations with Arrival = Date
    public int? ReservationRoomsTodayDone { get; set; } // Count of Rooms with Arrival = Date
    public int? ReservationAdultTodayDone { get; set; } // Count of Pax with Arrival = Date
    public int? ReservationChildTodayDone { get; set; } // Count of Child with Arrival = Date
    public int? ReservationExtraBedTodayDone { get; set; } // Count of ExtraBed with Arrival = Date

}