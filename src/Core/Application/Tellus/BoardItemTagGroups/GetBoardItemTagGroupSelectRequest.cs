using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.BoardItemTagGroups;
public class GetBoardItemTagGroupSelectRequest : IRequest<List<BoardItemTagGroupDto>>
{
    public int MandantId { get; set; }
    public GetBoardItemTagGroupSelectRequest(int mandantId)
    {
        MandantId = mandantId;
    }
}

public class GetBoardItemTagGroupSelectRequestHandler : IRequestHandler<GetBoardItemTagGroupSelectRequest, List<BoardItemTagGroupDto>>
{
    private readonly IRepository<BoardItemTagGroup> _repository;
    private readonly IStringLocalizer<GetBoardItemTagGroupSelectRequestHandler> _localizer;

    public GetBoardItemTagGroupSelectRequestHandler(IRepository<BoardItemTagGroup> repository, IStringLocalizer<GetBoardItemTagGroupSelectRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<BoardItemTagGroupDto>> Handle(GetBoardItemTagGroupSelectRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemTagGroupSelectSpecification(request.MandantId);
        var boardItemTagGroups = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemTagGroups is null) throw new NotFoundException(_localizer["boarditemtaggroup.notfound"]);

        return boardItemTagGroups;
    }
}

public class BoardItemTagGroupSelectSpecification : Specification<BoardItemTagGroup, BoardItemTagGroupDto>
{
    public BoardItemTagGroupSelectSpecification(int mandantId) =>
        Query.Where(x => x.MandantId == mandantId);
}