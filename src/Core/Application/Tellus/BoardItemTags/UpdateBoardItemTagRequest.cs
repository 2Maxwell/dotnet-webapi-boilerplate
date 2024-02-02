using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItemTags;
public class UpdateBoardItemTagRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Text { get; set; }
    public int BoardItemTagGroupId { get; set; }
}

public class UpdateBoardItemTagRequestValidator : CustomValidator<UpdateBoardItemTagRequest>
{
    public UpdateBoardItemTagRequestValidator(IReadRepository<BoardItemTag> repository, IStringLocalizer<UpdateBoardItemTagRequestValidator> localizer)
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (boardItemTag, text, ct) =>
                                                  await repository.GetBySpecAsync(new BoardItemTagByTextSpec(text, boardItemTag.MandantId), ct)
                                                        is not BoardItemTag existing || existing.Id == boardItemTag.Id)
                    .WithMessage((_, text) => string.Format(localizer["boardItemTagText.alreadyexists"], text));

        RuleFor(x => x.BoardItemTagGroupId)
            .NotEmpty();
    }
}

public class UpdateBoardItemTagRequestHandler : IRequestHandler<UpdateBoardItemTagRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemTag> _repository;
    private readonly IStringLocalizer<UpdateBoardItemTagRequestHandler> _localizer;

    public UpdateBoardItemTagRequestHandler(IRepositoryWithEvents<BoardItemTag> repository, IStringLocalizer<UpdateBoardItemTagRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardItemTagRequest request, CancellationToken cancellationToken)
    {
        var boardItemTag = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (boardItemTag is null) throw new NotFoundException(string.Format(_localizer["boardItemTag.notfound"], request.Id));

        boardItemTag.Update(request.Text, request.BoardItemTagGroupId);
        boardItemTag.DomainEvents.Add(EntityUpdatedEvent.WithEntity(boardItemTag));
        await _repository.UpdateAsync(boardItemTag, cancellationToken);
        return boardItemTag.Id;
    }
}
