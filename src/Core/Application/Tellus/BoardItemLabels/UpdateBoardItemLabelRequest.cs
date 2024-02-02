using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItemLabels;
public class UpdateBoardItemLabelRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Text { get; set; }
    public string? Color { get; set; }
    public bool DefaultLabel { get; set; }
    public int BoardItemLabelGroupId { get; set; }
}

public class UpdateBoardItemLabelRequestValidator : CustomValidator<UpdateBoardItemLabelRequest>
{
    public UpdateBoardItemLabelRequestValidator(IReadRepository<BoardItemLabel> repository, IStringLocalizer<UpdateBoardItemLabelRequestValidator> localizer)
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (boardItemLabel, text, ct) =>
                                       await repository.GetBySpecAsync(new BoardItemLabelByTextSpec(text, boardItemLabel.MandantId), ct)
                                                                  is not BoardItemLabel existing || existing.Id == boardItemLabel.Id)
                    .WithMessage((_, text) => string.Format(localizer["boardItemLabelText.alreadyexists"], text));

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(25);
    }
}

public class UpdateBoardItemLabelRequestHandler : IRequestHandler<UpdateBoardItemLabelRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemLabel> _repository;
    private readonly IStringLocalizer<UpdateBoardItemLabelRequestHandler> _localizer;

    public UpdateBoardItemLabelRequestHandler(IRepositoryWithEvents<BoardItemLabel> repository, IStringLocalizer<UpdateBoardItemLabelRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardItemLabelRequest request, CancellationToken cancellationToken)
    {
        var boardItemLabel = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (boardItemLabel is null) throw new NotFoundException(string.Format(_localizer["boardItemLabel.notfound"], request.Id));

        boardItemLabel.Update(request.Text, request.Color, request.DefaultLabel);
        boardItemLabel.DomainEvents.Add(EntityUpdatedEvent.WithEntity(boardItemLabel));
        await _repository.UpdateAsync(boardItemLabel, cancellationToken);
        return boardItemLabel.Id;
    }
}
