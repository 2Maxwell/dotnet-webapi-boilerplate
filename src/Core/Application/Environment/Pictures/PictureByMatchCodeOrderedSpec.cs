using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Pictures;
public class PictureByMatchCodeOrderedSpec : Specification<Picture>, ISingleResultSpecification
{
    public PictureByMatchCodeOrderedSpec(string matchCode, int mandantId) =>
        Query
            .Where(x => x.MandantId == mandantId && x.MatchCode.Contains(matchCode))
            .OrderBy(x => x.OrderNumber);
}
