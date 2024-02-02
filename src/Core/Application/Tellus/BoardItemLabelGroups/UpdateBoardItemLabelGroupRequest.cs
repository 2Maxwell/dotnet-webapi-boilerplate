using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItemLabelGroups;
public class UpdateBoardItemLabelGroupRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string Title { get; set; } = null!;
}

public class UpdateBoardItemLabelGroupRequestValidator : CustomValidator<UpdateBoardItemLabelGroupRequest>
{
    public UpdateBoardItemLabelGroupRequestValidator(IReadRepository<BoardItemLabelGroup> repository, IStringLocalizer<UpdateBoardItemLabelGroupRequestValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.MandantId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(25)
            .MustAsync(async (boardItemLabelGroup, title, ct) =>
                                                        await repository.GetBySpecAsync(new BoardItemLabelGroupByTitleSpec(title, boardItemLabelGroup.MandantId), ct)
                                                        is not BoardItemLabelGroup existing || existing.Id == boardItemLabelGroup.Id)
                    .WithMessage((_, title) => string.Format(localizer["boardItemLabelGroupTitle.alreadyexists"], title));
    }
}

public class UpdateBoardItemLabelGroupRequestHandler : IRequestHandler<UpdateBoardItemLabelGroupRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemLabelGroup> _repository;
    private readonly IStringLocalizer<UpdateBoardItemLabelGroupRequestHandler> _localizer;

    public UpdateBoardItemLabelGroupRequestHandler(IRepositoryWithEvents<BoardItemLabelGroup> repository, IStringLocalizer<UpdateBoardItemLabelGroupRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardItemLabelGroupRequest request, CancellationToken cancellationToken)
    {
        var boardItemLabelGroup = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (boardItemLabelGroup is null) throw new NotFoundException(string.Format(_localizer["boardItemLabelGroup.notfound"], request.Id));

        boardItemLabelGroup.Update(request.Title!);
        boardItemLabelGroup.DomainEvents.Add(EntityUpdatedEvent.WithEntity(boardItemLabelGroup));
        await _repository.UpdateAsync(boardItemLabelGroup, cancellationToken);
        return boardItemLabelGroup.Id;
    }
}