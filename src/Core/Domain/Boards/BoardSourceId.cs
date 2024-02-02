using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Boards;

[NotMapped]
public class BoardSourceId
{
    public string? Source { get; set; } // Guest, Booker, Reservation, Company,
    public int SourceId { get; set; }
    public string? Search { get; set; } // Source|SourceId (Guest|112568, Reservation|10125, Company|9869)
}
