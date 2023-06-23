namespace FSH.WebApi.Application.Hotel.Reservations;
public class ReservationHelper
{
    public string getResKzUpdateCase(string resKzStart, string resKzNew)
    {
        string result = "default";

        if (resKzStart == "A")
        {
            switch (resKzNew)
            {
                case "A":
                    result = "A_Update"; //Offer -> Offer
                    break;
                case "P":
                    result = "A_P"; //Offer -> Pending
                    break;
                case "R":
                    result = "A_R"; //Offer -> Reservation
                    break;
                case "C":
                    result = "A_C"; //Offer -> CheckIn
                    break;
                case "S":
                    result = "A_S"; //Offer -> Cancel
                    break;

            }
        }

        if (resKzStart == "P")
        {
            switch (resKzNew)
            {
                case "P":
                    result = "Update"; //Pending -> Pending
                    break;
                case "R":
                    result = "P_R"; //Pending -> Reservation
                    break;
                case "C":
                    result = "P_C"; //Pending -> CheckIn
                    break;
                case "S":
                    result = "P_S"; //Pending -> Cancel
                    break;
            }
        }

        if (resKzStart == "R")
        {
            switch (resKzNew)
            {
                case "R":
                    result = "Update"; //Reservation -> Reservation
                    break;
                case "C":
                    result = "R_C"; //Reservation -> CheckIn
                    break;
                case "S":
                    result = "R_S"; //Reservation -> Cancel
                    break;

            }
        }

        if (resKzStart == "C")
        {
            switch (resKzNew)
            {
                case "C":
                    result = "Update"; //Update GastImHaus
                    break;
                case "R":
                    result = "C_R"; //CheckInRollBack
                    break;
                case "O":
                    result = "C_O"; //CheckOut
                    break;
            }
        }

        if (resKzStart == "O")
        {
            switch (resKzNew)
            {
                case "C":
                    result = "O_C"; //CheckOutRollBack
                    break;
            }

        }

        if (resKzStart == "S")
        {
            switch (resKzNew)
            {
                case "P":
                    result = "S_P"; //Storno -> Pending
                    break;
                case "R":
                    result = "S_R"; //Storno -> Reservation
                    break;
            }
        }

        return result;
    }

    public string getRoomUpdateCase(int roomIdStart, int roomIdNew, DateTime arrivalStart, DateTime arrivalNew, DateTime departureStart, DateTime departureNew)
    {
        string result = string.Empty;
        if (roomIdStart == 0 && roomIdNew == 0)
        {
            result = "noAction";
        }

        if (roomIdStart == 0 && roomIdNew != 0)
        {
            result = "newRoom";
        }

        if (roomIdStart != 0 && roomIdNew == 0)
        {
            result = "deleteRoom";
        }

        if (roomIdStart != 0 && roomIdNew != 0)
        {
            if (roomIdStart != roomIdNew)
            {
                result = "changeRoom";
            }
            else
            {
                if (arrivalStart != arrivalNew || departureStart != departureNew)
                {
                    result = "changeRoom";
                }
                else
                {
                    result = "noAction";
                }
            }
        }

        return result;
    }
}
