using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;
//public class RoomState : AuditableEntity<int>, IAggregateRoot
//{
//    [Required]
//    public int MandantId { get; set; }
//    [Required]
//    public int RoomId { get; set; }
//    public bool ArrExp { get; set; }
//    public bool ArrCi { get; set; }
//    public bool Occ { get; set; }
//    public bool DepExp { get; set; }
//    public bool DepOut { get; set; }

//    //Status für Zimmerreinigung
//    public bool Assigned { get; set; }
//    public int DirtyDays { get; set; }
//    public int AssignedId { get; set; } // Zimmermädchen zugewiesen
//    public bool IsCleaning { get; set; }
//    public bool PendingClean { get; set; }
//    public bool CleanChecked { get; set; }
//    public int MinutesOccupied { get; set; }
//    public int MinutesDeparture { get; set; }
//    public int MinutesDefault { get; set; }
//}

    // CleainingRhythmus klären möglicherweise über Pakete den Rhythmus
    // Reinigungart Bleibe oder Abreise
    // weitere Tabelle für die einzelnen Arbeitsgänge je Zimmer oder Category und die Zeiteinheiten Soll festlegen
    // externen Arbeiten die nicht in der Zimmerreinigung enthalten sind (öffentl. Toiletten, Restaurant saugen, ...)
    // räumliche Zuordnung der Zimmer oder Bereiche anlegen: Hotel.Neubau.Erdgeschoss, Villa1.Erdgeschoss,FlurRechts, Villa2.Obergeschoss,
    // Hotel.Restaurant, Hotel.Empfang.Toiletten, Hotel.Altbau.30er, ...
    // über die räumlich Zuordnung Zimmer zusammenstellen und Empfehlungen über die Aufgaben der Zimmermädchen geben
    // Zimmermädchen kann dann die Aufgabenliste abarbeiten und abhaken

    // Frontend Housekeeping
    // Frontend Zimmerreinigung
    // Frontend Hausdame

    // Funktionen im Zimmer
    // Zimmerstatus automatisch
    // Minibar
    // Maintenance (Repair)
    // Issues
    // Todos
    // Roomservice (Restaurant)
    // allg. Aufgaben
    // Checkliste einbinden

    // Vorgänge
    // NightAudit
    // alle Vorgange setzen
    // Reservierung mit Zimmer am Anreisetag
    // ArrExp,
    // CheckIn
    // ArrExp, ArrCi, Occ, Dirty, DirtyDays = 1
    // CheckOut
    // DepOut
