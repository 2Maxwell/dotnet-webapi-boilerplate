using FSH.WebApi.Application.Tellus.BoardItemLabels;
using FSH.WebApi.Application.Tellus.Boards;
using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardCollections;
public class CreateBoardCollectionRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<BoardItemLabelDto>? BoardItemLabels { get; set; } // zum laden und arbeiten im WASM wird mit BoardItemLabelIds geladen
    public string? BoardItemLabelIds { get; set; }
    public ICollection<BoardDto>? Boards { get; set; } // zum laden und arbeiten im WASM wird mit BoardIds geladen
    public string? BoardIds { get; set; }
    public bool UserOnly { get; set; }
}

public class CreateBoardCollectionRequestValidator : CustomValidator<CreateBoardCollectionRequest>
{
    public CreateBoardCollectionRequestValidator(IReadRepository<BoardCollection> repository, IStringLocalizer<CreateBoardCollectionRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
        .NotEmpty();

        RuleFor(x => x.Title)
        .NotEmpty()
        .MaximumLength(25)
        .MustAsync(async (boardCollection, title, ct) =>
                   await repository.GetBySpecAsync(new BoardCollectionByTitleSpec(title, boardCollection.MandantId), ct)
                   is null)
                .WithMessage((_, title) => string.Format(localizer["boardCollectionTitle.alreadyexists"], title));

        RuleFor(x => x.Description)
            .MaximumLength(100);
    }
}

public class BoardCollectionByTitleSpec : Specification<BoardCollection>, ISingleResultSpecification
{
    public BoardCollectionByTitleSpec(string title, int mandantId)
    {
        Query.Where(x => x.Title == title && x.MandantId == mandantId);
    }
}

public class CreateBoardCollectionRequestHandler : IRequestHandler<CreateBoardCollectionRequest, int>
{
    private readonly IRepositoryWithEvents<BoardCollection> _repository;
    private readonly IStringLocalizer<CreateBoardCollectionRequestHandler> _localizer;

    public CreateBoardCollectionRequestHandler(IRepositoryWithEvents<BoardCollection> repository, IStringLocalizer<CreateBoardCollectionRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardCollectionRequest request, CancellationToken cancellationToken)
    {
        var boardCollection = new BoardCollection(request.MandantId, request.Title!, request.Description, request.BoardItemLabelIds, request.BoardIds, request.UserOnly);
        //new BoardCollection(request.MandantId, request.Title!, request.Description, request.BoardItemLabels, request.Boards, request.UserOnly);
        boardCollection.DomainEvents.Add(EntityCreatedEvent.WithEntity(boardCollection));
        await _repository.AddAsync(boardCollection, cancellationToken);
        return boardCollection.Id;
    }
}