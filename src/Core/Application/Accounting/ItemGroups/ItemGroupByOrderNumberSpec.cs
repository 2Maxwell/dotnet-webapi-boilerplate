namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class ItemGroupByOrderNumberSpec : Specification<ItemGroup>, ISingleResultSpecification
{
    public ItemGroupByOrderNumberSpec(int orderNumber, int mandantId) =>
            Query.Where(i => i.OrderNumber == orderNumber && (i.MandantId == mandantId || i.MandantId == 0));
}