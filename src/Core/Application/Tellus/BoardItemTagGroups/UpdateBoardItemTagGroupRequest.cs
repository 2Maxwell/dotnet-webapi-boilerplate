using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItemTagGroups;
public class UpdateBoardItemTagGroupRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Color { get; set; }
}

public class UpdateBoardItemTagGroupRequestValidator : CustomValidator<UpdateBoardItemTagGroupRequest>
{
    public UpdateBoardItemTagGroupRequestValidator(IReadRepository<BoardItemTagGroup> repository, IStringLocalizer<UpdateBoardItemTagGroupRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(25)
            .MustAsync(async (boardItemTagGroup, title, ct) =>
                                             await repository.GetBySpecAsync(new BoardItemTagGroupByTitleSpec(title, boardItemTagGroup.MandantId), ct)
                                             is not BoardItemTagGroup existing || existing.Id == boardItemTagGroup.Id)
                    .WithMessage((_, title) => string.Format(localizer["boardItemTagGroupTitle.alreadyexists"], title));

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(25);
    }
}

public class UpdateBoardItemTagGroupRequestHandler : IRequestHandler<UpdateBoardItemTagGroupRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemTagGroup> _repository;
    private readonly IStringLocalizer<UpdateBoardItemTagGroupRequestHandler> _localizer;

    public UpdateBoardItemTagGroupRequestHandler(IRepositoryWithEvents<BoardItemTagGroup> repository, IStringLocalizer<UpdateBoardItemTagGroupRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardItemTagGroupRequest request, CancellationToken cancellationToken)
    {
        var boardItemTagGroup = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (boardItemTagGroup is null) throw new NotFoundException(string.Format(_localizer["boardItemTagGroup.notfound"], request.Id));

        boardItemTagGroup.Update(request.Title!, request.Color!);
        boardItemTagGroup.DomainEvents.Add(EntityUpdatedEvent.WithEntity(boardItemTagGroup));
        await _repository.UpdateAsync(boardItemTagGroup, cancellationToken);
        return boardItemTagGroup.Id;
    }
}
