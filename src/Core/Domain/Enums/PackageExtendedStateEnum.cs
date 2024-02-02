namespace FSH.WebApi.Domain.Enums;
public enum PackageExtendedStateEnum
{
                    // PackageRhythmus per Appointment !!!!
    cartItem = 100, // im CartItem vorhanden noch keine Reservierung
    pending = 200,  // in einer Reservierung vorhanden noch kein Appointment eingetragen
    reserved = 300, // in einer Reservierung vorhanden Appointment eingetragen aber noch nicht stattgefunden
    booked = 400,   // gebucht bsp. bei einer Reservierung auch wenn es nicht mehr stattfindet muss es gebucht werden
}
