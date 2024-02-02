using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItems;
public class CreateBoardItemRequest : IRequest<int>
{
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
    public bool IsTemplate { get; set; }
    public bool? FixedBoard { get; set; }
    public string? ImageName { get; set; }
    public string? ImagePath { get; set; }
    public string? RepeatMatchCode { get; set; }
    public string? BoardItemRepeaterJson { get; set; }
    public string? SourceLink { get; set; }
    public int? DefaultBoardItemLabelGroupId { get; set; }
}

public class CreateBoardItemRequestValidator : CustomValidator<CreateBoardItemRequest>
{
    public CreateBoardItemRequestValidator(IReadRepository<BoardItem> repository, IStringLocalizer<CreateBoardItemRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(40);
        RuleFor(x => x.Description)
            .MaximumLength(200);
        RuleFor(x => x.BoardItemLabelIds)
            .MaximumLength(400);
        RuleFor(x => x.BoardItemTagIds)
            .MaximumLength(400);
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

public class BoardItemByTitleSpec : Specification<BoardItem>, ISingleResultSpecification
{
    public BoardItemByTitleSpec(string title, int mandantId)
    {
        Query.Where(x => x.Title == title && x.MandantId == mandantId);
    }
}

public class CreateBoardItemRequestHandler : IRequestHandler<CreateBoardItemRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItem> _repository;
    private readonly IStringLocalizer<CreateBoardItemRequestHandler> _localizer;

    public CreateBoardItemRequestHandler(IRepositoryWithEvents<BoardItem> repository, IStringLocalizer<CreateBoardItemRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardItemRequest request, CancellationToken cancellationToken)
    {
        var boardItem = new BoardItem(request.MandantId, request.BoardItemLabelIds, request.BoardItemTagIds, request.Title!, request.Description, request.BoardId, request.Start, request.End, request.BoardItemTypeEnumId, request.BoardSourceIdJson, request.IsTemplate, request.FixedBoard, request.ImageName, request.ImagePath, request.RepeatMatchCode, request.BoardItemRepeaterJson, request.SourceLink, request.DefaultBoardItemLabelGroupId);
        boardItem.DomainEvents.Add(EntityCreatedEvent.WithEntity(boardItem));
        await _repository.AddAsync(boardItem, cancellationToken);
        // await _repository.SaveChangesAsync(cancellationToken);
        return boardItem.Id;
    }
}