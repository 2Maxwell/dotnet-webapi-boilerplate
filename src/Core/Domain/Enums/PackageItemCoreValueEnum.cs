using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Enums;

public enum PackageItemCoreValueEnum
{
    OptionalWithPercentage = 1, // Wenn ein Preis im Shop eingegeben werden kann
    PriceLine = 50,              // Ein Preis wird ohne ItemId eingegeben dann muss eine prozentuale Verteilung eingegeben werden.
    Roomrate = 100
}
