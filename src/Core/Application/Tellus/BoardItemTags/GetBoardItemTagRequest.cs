using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardItemTags;
public class GetBoardItemTagRequest : IRequest<BoardItemTagDto>
{
    public int Id { get; set; }
    public GetBoardItemTagRequest(int id) => Id = id;
}

public class GetBoardItemTagRequestHandler : IRequestHandler<GetBoardItemTagRequest, BoardItemTagDto>
{
    private readonly IRepository<BoardItemTag> _repository;
    private readonly IRepository<BoardItemTagGroup> _repositoryBoardItemTagGroup;
    private readonly IStringLocalizer<GetBoardItemTagRequest> _localizer;

    public GetBoardItemTagRequestHandler(IRepository<BoardItemTag> repository, IRepository<BoardItemTagGroup> repositoryBoardItemTagGroup, IStringLocalizer<GetBoardItemTagRequest> localizer)
    {
        _repository = repository;
        _repositoryBoardItemTagGroup = repositoryBoardItemTagGroup;
        _localizer = localizer;
    }

    public async Task<BoardItemTagDto> Handle(GetBoardItemTagRequest request, CancellationToken cancellationToken)
    {
        var boardItemTag = (await _repository.GetByIdAsync(request.Id, cancellationToken))!.Adapt<BoardItemTagDto>()
        ?? throw new NotFoundException(string.Format(_localizer["boarditemtag.notfound"], request.Id));

        // boardItemTag.Color aus der BoardItemTagGroup holen
        var boardItemTagGroup = await _repositoryBoardItemTagGroup.GetByIdAsync(boardItemTag.BoardItemTagGroupId, cancellationToken);
        boardItemTag.Color = boardItemTagGroup!.Color;

        return boardItemTag;
    }
}
