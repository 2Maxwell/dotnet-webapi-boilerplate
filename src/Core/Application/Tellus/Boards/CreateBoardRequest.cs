using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.Boards;
public class CreateBoardRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public bool UserOnly { get; set; }
    public ICollection<BoardItem>? BoardItems { get; set; }
    public int? BoardLabelAdd { get; set; } // wird dem BoardItem hinzugefügt Dirty -> Clean
    public string? BoardLabelRemove { get; set; } // LabelText mit Komma getrennt wird dem BoardItem entfernt
    public bool DoneBoard { get; set; } // wenn hier hin verschoben wird dann wird done = true gesetzt
    public int DefaultBoardItemLabelGroupId { get; set; }
    public string? Source { get; set; } // Reservation, Guest, Company, Tellus
    public int? SourceId { get; set; }

}

public class CreateBoardRequestValidator : CustomValidator<CreateBoardRequest>
{
    public CreateBoardRequestValidator(IReadRepository<Board> repository, IStringLocalizer<CreateBoardRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
        .NotEmpty();
        RuleFor(x => x.Title)
        .NotEmpty()
        .MaximumLength(25)
        .MustAsync(async (board, title, ct) => await repository.GetBySpecAsync(new BoardByTitleSpec(title, board.MandantId), ct) is null)
                .WithMessage((_, title) => string.Format(localizer["boardTitle.alreadyexists"], title));
        RuleFor(x => x.Description)
            .MaximumLength(100);
        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(15);
        RuleFor(x => x.BoardLabelRemove)
            .MaximumLength(60);
        RuleFor(x => x.DefaultBoardItemLabelGroupId)
            .GreaterThan(0);

    }
}

public class BoardByTitleSpec : Specification<Board>, ISingleResultSpecification
{
    public BoardByTitleSpec(string title, int mandantId)
    {
        Query.Where(x => x.Title == title && x.MandantId == mandantId);
    }
}

public class CreateBoardRequestHandler : IRequestHandler<CreateBoardRequest, int>
{
    private readonly IRepositoryWithEvents<Board> _repository;
    private readonly IStringLocalizer<CreateBoardRequestHandler> _localizer;

    public CreateBoardRequestHandler(IRepositoryWithEvents<Board> repository, IStringLocalizer<CreateBoardRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardRequest request, CancellationToken cancellationToken)
    {
        // var board = new Board(request.MandantId, request.Title!, request.Description, request.Color!, request.UserOnly, request.BoardLabelAdd, request.BoardLabelRemove, request.DoneBoard);
        var board = new Board(request.MandantId, request.Title!, request.Description, request.Color!, request.UserOnly, request.BoardLabelAdd, request.BoardLabelRemove, request.DoneBoard, request.DefaultBoardItemLabelGroupId, request.Source, request.SourceId);
        board.DomainEvents.Add(EntityCreatedEvent.WithEntity(board));
        await _repository.AddAsync(board, cancellationToken);
        return board.Id;
    }
}