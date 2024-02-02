using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardItemTagGroups;
public class GetBoardItemTagGroupRequest : IRequest<BoardItemTagGroupDto>
{
    public int Id { get; set; }
    public GetBoardItemTagGroupRequest(int id) => Id = id;
}

public class GetBoardItemTagGroupRequestHandler : IRequestHandler<GetBoardItemTagGroupRequest, BoardItemTagGroupDto>
{
    private readonly IRepository<BoardItemTagGroup> _repository;
    private readonly IStringLocalizer<GetBoardItemTagGroupRequest> _localizer;

    public GetBoardItemTagGroupRequestHandler(IRepository<BoardItemTagGroup> repository, IStringLocalizer<GetBoardItemTagGroupRequest> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<BoardItemTagGroupDto> Handle(GetBoardItemTagGroupRequest request, CancellationToken cancellationToken)
    {
        var boardItemTagGroup = (await _repository.GetByIdAsync(request.Id, cancellationToken))!.Adapt<BoardItemTagGroupDto>()
        ?? throw new NotFoundException(string.Format(_localizer["boarditemtaggroup.notfound"], request.Id));

        return boardItemTagGroup;
    }
}
