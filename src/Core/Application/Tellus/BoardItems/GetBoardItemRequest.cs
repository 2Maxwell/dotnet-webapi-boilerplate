using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardItems;
public class GetBoardItemRequest : IRequest<BoardItemDto>
{
    public int Id { get; set; }
    public GetBoardItemRequest(int id) => Id = id;
}

public class GetBoardItemRequestHandler : IRequestHandler<GetBoardItemRequest, BoardItemDto>
{
    private readonly IRepository<BoardItem> _repository;
    private readonly IStringLocalizer<GetBoardItemRequestHandler> _localizer;

    public GetBoardItemRequestHandler(IRepository<BoardItem> repository, IStringLocalizer<GetBoardItemRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<BoardItemDto> Handle(GetBoardItemRequest request, CancellationToken cancellationToken)
    {
        var boardItem = (await _repository.GetByIdAsync(request.Id, cancellationToken))!.Adapt<BoardItemDto>()
        ?? throw new NotFoundException(string.Format(_localizer["boarditem.notfound"], request.Id));

        return boardItem;
    }
}
