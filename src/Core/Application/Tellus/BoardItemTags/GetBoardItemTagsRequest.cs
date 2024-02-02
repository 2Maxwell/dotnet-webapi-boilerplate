using FSH.WebApi.Domain.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Tellus.BoardItemTags;
public class GetBoardItemTagsRequest : IRequest<List<BoardItemTagDto>>
{
    public int MandantId { get; set; }
}

public class GetBoardItemTagsRequestHandler : IRequestHandler<GetBoardItemTagsRequest, List<BoardItemTagDto>>
{
    private readonly IRepository<BoardItemTag> _repository;
    private readonly IStringLocalizer<GetBoardItemTagsRequestHandler> _localizer;

    public GetBoardItemTagsRequestHandler(IRepository<BoardItemTag> repository, IStringLocalizer<GetBoardItemTagsRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<BoardItemTagDto>> Handle(GetBoardItemTagsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemTagSpecification(request.MandantId);
        var boardItemTags = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemTags is null) throw new NotFoundException(_localizer["boarditemtag.notfound"]);

        return boardItemTags;
    }
}

internal class BoardItemTagSpecification : Specification<BoardItemTag, BoardItemTagDto>
{
    public BoardItemTagSpecification(int mandantId) =>
        Query.Where(x => x.MandantId == mandantId);
}