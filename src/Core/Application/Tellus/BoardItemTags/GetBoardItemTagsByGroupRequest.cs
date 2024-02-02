using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardItemTags;
public class GetBoardItemTagsByGroupRequest : IRequest<List<BoardItemTagDto>>
{
    public int MandantId { get; set; }
    public int BoardItemTagGroupId { get; set; }
}

public class GetBoardItemTagsByGroupRequestHandler : IRequestHandler<GetBoardItemTagsByGroupRequest, IEnumerable<BoardItemTagDto>>
{
    private readonly IRepository<BoardItemTag> _repository;
    private readonly IRepository<BoardItemTagGroup> _repositoryBoardItemTagGroup;
    private readonly IStringLocalizer<GetBoardItemTagsByGroupRequest> _localizer;

    public GetBoardItemTagsByGroupRequestHandler(IRepository<BoardItemTag> repository, IRepository<BoardItemTagGroup> repositoryBoardItemTagGroup, IStringLocalizer<GetBoardItemTagsByGroupRequest> localizer)
    {
        _repository = repository;
        _repositoryBoardItemTagGroup = repositoryBoardItemTagGroup;
        _localizer = localizer;
    }

    public async Task<IEnumerable<BoardItemTagDto>> Handle(GetBoardItemTagsByGroupRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemTagByGroupSpecification(request.MandantId, request.BoardItemTagGroupId);
        var boardItemTags = (await _repository.ListAsync(spec, cancellationToken)).Adapt<List<BoardItemTagDto>>()
        ?? throw new NotFoundException(string.Format(_localizer["boarditemtags.notfound"], request.BoardItemTagGroupId));

        // boardItemTag.Color aus der BoardItemTagGroup holen
        var boardItemTagGroup = await _repositoryBoardItemTagGroup.GetByIdAsync(request.BoardItemTagGroupId, cancellationToken);
        var color = boardItemTagGroup!.Color;

        foreach (var boardItemTag in boardItemTags)
        {
            boardItemTag.Color = color;
        }

        return boardItemTags;
    }
}

public class BoardItemTagByGroupSpecification : Specification<BoardItemTag, BoardItemTagDto>, ISingleResultSpecification
{
    public BoardItemTagByGroupSpecification(int mandantId, int boardItemTagGroupId) =>
        Query.Where(x => x.MandantId == mandantId && x.BoardItemTagGroupId == boardItemTagGroupId);
}