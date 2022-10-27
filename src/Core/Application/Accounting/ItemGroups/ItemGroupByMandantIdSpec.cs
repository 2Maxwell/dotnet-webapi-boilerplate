namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class ItemGroupByMandantIdSpec : Specification<ItemGroup, ItemGroupDto>
{
    public ItemGroupByMandantIdSpec(int mandantId) =>
        Query.Where(i => i.MandantId == 0 || i.MandantId == mandantId);
}