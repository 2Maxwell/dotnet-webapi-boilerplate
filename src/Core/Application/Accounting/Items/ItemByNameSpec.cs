namespace FSH.WebApi.Application.Accounting.Items;

public class ItemByNameSpec : Specification<Item>, ISingleResultSpecification
{
    public ItemByNameSpec(string name, int mandantId) =>
        Query.Where(i => i.Name == name && (i.MandantId == mandantId || i.MandantId == 0));
}