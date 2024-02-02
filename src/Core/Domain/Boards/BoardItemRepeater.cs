using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Boards;

[NotMapped]
public class BoardItemRepeater
{
    public string? RepeatInterval { get; set; } // Daily, Weekly, Monthly, Yearly
    public TimeOnly? RepeatTime { get; set; }
    public string? RepeatWeekday { get; set; } // MO, DI, MI, DO, FR, SA, SO
    public string? RepeatDay { get; set; } // 1-31 mit Komma getrennt bei Mehrfachauswahl
    public string? RepeatMonth { get; set; } // 1-12 mit Komma getrennt bei Mehrfachauswahl
    public DateTime? RepeatLast { get; set; } // letztes Datum der Wiederholung, ohne Wert = immer
}
