using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItemLabels;
public class CreateBoardItemLabelRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Text { get; set; }
    public string? Color { get; set; }
    public bool DefaultLabel { get; set; }
    public int BoardItemLabelGroupId { get; set; }
}

public class CreateBoardItemLabelRequestValidator : CustomValidator<CreateBoardItemLabelRequest>
{
    public CreateBoardItemLabelRequestValidator(IReadRepository<BoardItemLabel> repository, IStringLocalizer<CreateBoardItemLabelRequestValidator> localizer)
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (boardItemLabel, text, ct) =>
                                  await repository.GetBySpecAsync(new BoardItemLabelByTextSpec(text, boardItemLabel.MandantId), ct)
                                                        is null)
                    .WithMessage((_, text) => string.Format(localizer["boardItemLabelText.alreadyexists"], text));

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(25);
    }
}

public class BoardItemLabelByTextSpec : Specification<BoardItemLabel>, ISingleResultSpecification
{
    public BoardItemLabelByTextSpec(string text, int mandantId)
    {
        Query.Where(x => x.Text == text && x.MandantId == mandantId);
    }
}

public class CreateBoardItemLabelRequestHandler : IRequestHandler<CreateBoardItemLabelRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemLabel> _repository;
    private readonly IStringLocalizer<CreateBoardItemLabelRequestHandler> _localizer;

    public CreateBoardItemLabelRequestHandler(IRepositoryWithEvents<BoardItemLabel> repository, IStringLocalizer<CreateBoardItemLabelRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardItemLabelRequest request, CancellationToken cancellationToken)
    {
        var boardItemLabel = new BoardItemLabel(request.MandantId, request.Text!, request.Color!, request.DefaultLabel, request.BoardItemLabelGroupId );
        boardItemLabel.DomainEvents.Add(EntityCreatedEvent.WithEntity(boardItemLabel));
        await _repository.AddAsync(boardItemLabel, cancellationToken);
        return boardItemLabel.Id;
    }
}