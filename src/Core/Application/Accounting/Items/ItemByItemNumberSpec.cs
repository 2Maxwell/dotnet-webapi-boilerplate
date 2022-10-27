namespace FSH.WebApi.Application.Accounting.Items;

public class ItemByItemNumberSpec : Specification<Item>, ISingleResultSpecification
{
    public ItemByItemNumberSpec(int itemNumber, int mandantId) =>
        Query.Where(i => i.ItemNumber == itemNumber && (i.MandantId == mandantId || i.MandantId == 0));
}