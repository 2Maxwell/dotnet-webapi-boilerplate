using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItemTags;
public class CreateBoardItemTagRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Text { get; set; }
    public int BoardItemTagGroupId { get; set; }
}

public class CreateBoardItemTagRequestValidator : CustomValidator<CreateBoardItemTagRequest>
{
    public CreateBoardItemTagRequestValidator(IReadRepository<BoardItemTag> repository, IStringLocalizer<CreateBoardItemTagRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
        .NotEmpty();

        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (boardItemTag, text, ct) =>
                                  await repository.GetBySpecAsync(new BoardItemTagByTextSpec(text, boardItemTag.MandantId), ct)
                                                        is null)
                    .WithMessage((_, text) => string.Format(localizer["boardItemTagText.alreadyexists"], text));

        RuleFor(x => x.BoardItemTagGroupId)
            .NotEmpty();
    }
}

public class BoardItemTagByTextSpec : Specification<BoardItemTag>, ISingleResultSpecification
{
    public BoardItemTagByTextSpec(string text, int mandantId)
    {
        Query.Where(x => x.Text == text && x.MandantId == mandantId);
    }
}

public class CreateBoardItemTagRequestHandler : IRequestHandler<CreateBoardItemTagRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemTag> _repository;
    private readonly IStringLocalizer<CreateBoardItemTagRequestHandler> _localizer;

    public CreateBoardItemTagRequestHandler(IRepositoryWithEvents<BoardItemTag> repository, IStringLocalizer<CreateBoardItemTagRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardItemTagRequest request, CancellationToken cancellationToken)
    {
        var boardItemTag = new BoardItemTag(request.MandantId, request.Text!, request.BoardItemTagGroupId);
        boardItemTag.DomainEvents.Add(EntityCreatedEvent.WithEntity(boardItemTag));
        await _repository.AddAsync(boardItemTag, cancellationToken);
        return boardItemTag.Id;
    }
}