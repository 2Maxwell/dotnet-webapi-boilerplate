using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItems;
public class DeleteBoardItemRequest : IRequest<int>
{
    public DeleteBoardItemRequest(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class DeleteBoardItemRequestHandler : IRequestHandler<DeleteBoardItemRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItem> _repository;
    private readonly IStringLocalizer<DeleteBoardItemRequestHandler> _localizer;

    public DeleteBoardItemRequestHandler(IRepositoryWithEvents<BoardItem> repository, IStringLocalizer<DeleteBoardItemRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(DeleteBoardItemRequest request, CancellationToken cancellationToken)
    {
        var boardItem = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = boardItem ?? throw new NotFoundException(_localizer["BoardItem {0} Not Found."]);

        // Add Domain Events to be raised after the commit
        boardItem.DomainEvents.Add(EntityDeletedEvent.WithEntity(boardItem));

        await _repository.DeleteAsync(boardItem, cancellationToken);

        return request.Id;
    }
}
