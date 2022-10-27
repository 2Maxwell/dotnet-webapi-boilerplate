namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class ItemGroupByNameSpec : Specification<ItemGroup>, ISingleResultSpecification
{
    public ItemGroupByNameSpec(string name, int mandantId) =>
        Query.Where(i => i.Name == name && (i.MandantId == mandantId || i.MandantId == 0));
}