using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItems;
public class UpdateBoardItemRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? BoardItemLabelIds { get; set; }
    public string? BoardItemTagIds { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? BoardId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int? BoardItemTypeEnumId { get; set; }
    public string? BoardSourceIdJson { get; set; }
    public List<BoardSourceId>? BoardSourceIds { get; set; }
    public bool IsTemplate { get; set; }
    public bool? FixedBoard { get; set; }
    public string? ImageName { get; set; }
    public string? ImagePath { get; set; }
    public string? RepeatMatchCode { get; set; }
    public string? BoardItemRepeaterJson { get; set; }
    public BoardItemRepeater? BoardItemRepeater { get; set; }
    public string? SourceLink { get; set; }
    public int? DefaultBoardItemLabelGroupId { get; set; }
}

public class UpdateBoardItemRequestValidator : CustomValidator<UpdateBoardItemRequest>
{
    public UpdateBoardItemRequestValidator(IReadRepository<BoardItem> repository, IStringLocalizer<UpdateBoardItemRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
        .NotEmpty()
        .MaximumLength(40);
        RuleFor(x => x.Description)
            .MaximumLength(200);
        RuleFor(x => x.ImageName)
            .MaximumLength(40);
        RuleFor(x => x.ImagePath)
            .MaximumLength(200);
        RuleFor(x => x.RepeatMatchCode)
            .MaximumLength(40);
        RuleFor(x => x.SourceLink)
            .MaximumLength(200);
        RuleFor(x => x.DefaultBoardItemLabelGroupId)
            .GreaterThan(0);

    }
}

public class UpdateBoardItemRequestHandler : IRequestHandler<UpdateBoardItemRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItem> _repository;
    private readonly IStringLocalizer<UpdateBoardItemRequestHandler> _localizer;

    public UpdateBoardItemRequestHandler(IRepositoryWithEvents<BoardItem> repository, IStringLocalizer<UpdateBoardItemRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardItemRequest request, CancellationToken cancellationToken)
    {
        var boardItem = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (boardItem is null) throw new NotFoundException(string.Format(_localizer["boardItem.notfound"], request.Id));

        boardItem.Update(request.BoardItemLabelIds, request.BoardItemTagIds, request.Title!, request.Description, request.BoardId, request.Start, request.End, request.BoardItemTypeEnumId, request.BoardSourceIdJson, request.IsTemplate, request.FixedBoard, request.ImageName, request.ImagePath, request.RepeatMatchCode, request.BoardItemRepeaterJson, request.SourceLink, request.DefaultBoardItemLabelGroupId);
        boardItem.DomainEvents.Add(EntityUpdatedEvent.WithEntity(boardItem));
        await _repository.UpdateAsync(boardItem, cancellationToken);
        return boardItem.Id;
    }
}
