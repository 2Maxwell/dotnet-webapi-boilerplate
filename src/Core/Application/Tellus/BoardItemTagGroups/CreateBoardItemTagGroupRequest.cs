using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItemTagGroups;
public class CreateBoardItemTagGroupRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Color { get; set; }
}

public class CreateBoardItemTagGroupRequestValidator : CustomValidator<CreateBoardItemTagGroupRequest>
{
    public CreateBoardItemTagGroupRequestValidator(IReadRepository<BoardItemTagGroup> repository, IStringLocalizer<CreateBoardItemTagGroupRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
        .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(25)
            .MustAsync(async (boardItemTagGroup, title, ct) =>
                                  await repository.GetBySpecAsync(new BoardItemTagGroupByTitleSpec(title, boardItemTagGroup.MandantId), ct)
                                                        is null)
                    .WithMessage((_, title) => string.Format(localizer["boardItemTagGroupTitle.alreadyexists"], title));

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(25);
    }
}

public class BoardItemTagGroupByTitleSpec : Specification<BoardItemTagGroup>, ISingleResultSpecification
{
    public BoardItemTagGroupByTitleSpec(string title, int mandantId)
    {
        Query.Where(x => x.Title == title && x.MandantId == mandantId);
    }
}

public class CreateBoardItemTagGroupRequestHandler : IRequestHandler<CreateBoardItemTagGroupRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemTagGroup> _repository;
    private readonly IStringLocalizer<CreateBoardItemTagGroupRequestHandler> _localizer;

    public CreateBoardItemTagGroupRequestHandler(IRepositoryWithEvents<BoardItemTagGroup> repository, IStringLocalizer<CreateBoardItemTagGroupRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardItemTagGroupRequest request, CancellationToken cancellationToken)
    {
        var boardItemTagGroup = new BoardItemTagGroup(request.MandantId, request.Title!, request.Color!);
        boardItemTagGroup.DomainEvents.Add(EntityCreatedEvent.WithEntity(boardItemTagGroup));
        await _repository.AddAsync(boardItemTagGroup, cancellationToken);
        return boardItemTagGroup.Id;
    }
}