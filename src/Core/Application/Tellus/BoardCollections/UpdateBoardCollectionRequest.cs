using FSH.WebApi.Application.Tellus.BoardItemLabels;
using FSH.WebApi.Application.Tellus.Boards;
using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardCollections;
public class UpdateBoardCollectionRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? BoardItemLabelIds { get; set; }
    public ICollection<BoardItemLabelDto>? BoardItemLabels { get; set; }
    public string? BoardIds { get; set; }
    public ICollection<BoardDto>? Boards { get; set; } // zum laden und arbeiten im WASM wird mit BoardIds geladen
    public bool UserOnly { get; set; }
}

public class UpdateBoardCollectionRequestValidator : CustomValidator<UpdateBoardCollectionRequest>
{
    public UpdateBoardCollectionRequestValidator(IReadRepository<BoardCollection> repository, IStringLocalizer<UpdateBoardCollectionRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
        .NotEmpty()
        .MaximumLength(25)
        .MustAsync(async (boardCollection, title, ct) =>
                        await repository.GetBySpecAsync(new BoardCollectionByTitleSpec(title, boardCollection.MandantId), ct)
                        is not BoardCollection existing || existing.Id == boardCollection.Id)
                .WithMessage((_, title) => string.Format(localizer["boardCollectionTitle.alreadyexists"], title));

        RuleFor(x => x.Description)
            .MaximumLength(100);
    }
}

public class UpdateBoardCollectionRequestHandler : IRequestHandler<UpdateBoardCollectionRequest, int>
{
    private readonly IRepositoryWithEvents<BoardCollection> _repository;
    private readonly IStringLocalizer<UpdateBoardCollectionRequestHandler> _localizer;

    public UpdateBoardCollectionRequestHandler(IRepositoryWithEvents<BoardCollection> repository, IStringLocalizer<UpdateBoardCollectionRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardCollectionRequest request, CancellationToken cancellationToken)
    {
        var boardCollection = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (boardCollection is null) throw new NotFoundException(string.Format(_localizer["boardCollection.notfound"], request.Id));

        boardCollection.Update(request.Title!, request.Description, request.BoardItemLabelIds, request.BoardIds, request.UserOnly);
        boardCollection.DomainEvents.Add(EntityUpdatedEvent.WithEntity(boardCollection));
        await _repository.UpdateAsync(boardCollection, cancellationToken);
        return boardCollection.Id;
    }
}
