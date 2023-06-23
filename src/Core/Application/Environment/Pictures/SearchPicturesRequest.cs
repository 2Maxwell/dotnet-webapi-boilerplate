using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.Pictures;
public class SearchPicturesRequest : IRequest<List<PictureDto>>
{
    public int MandantId { get; set; }
    public string? MatchCode { get; set; }
    public bool AllMatchCodes { get; set; }
}

public class PicturesBySearchRequestSpec : Specification<Picture, PictureDto>
{
    public PicturesBySearchRequestSpec(SearchPicturesRequest request)
    {
        if (request.AllMatchCodes)
        {
            Query
            .Where(c => c.MandantId == request.MandantId)
            .OrderBy(c => c.OrderNumber);
        }
        else
        {
            Query
                .Where(c => c.MatchCode.Contains(request.MatchCode))
                .OrderBy(c => c.OrderNumber);
        }
    }
}

public class SearchPicturesRequestHandler : IRequestHandler<SearchPicturesRequest, List<PictureDto>>
{
    private readonly IReadRepository<Picture> _repository;

    public SearchPicturesRequestHandler(IReadRepository<Picture> repository) => _repository = repository;

    public async Task<List<PictureDto>> Handle(SearchPicturesRequest request, CancellationToken cancellationToken)
    {
        var spec = new PicturesBySearchRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);
        return list;
    }
}
