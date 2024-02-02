using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.Boards;
public class UpdateBoardRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public bool UserOnly { get; set; }
    public int? BoardLabelAdd { get; set; }
    public string? BoardLabelRemove { get; set; }
    public bool DoneBoard { get; set; }
    public int DefaultBoardItemLabelGroupId { get; set; }
}

public class UpdateBoardRequestValidator : CustomValidator<UpdateBoardRequest>
{
    public UpdateBoardRequestValidator(IReadRepository<Board> repository, IStringLocalizer<UpdateBoardRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
        .NotEmpty()
        .MaximumLength(25)
        .MustAsync(async (board, title, ct) =>
            await repository.GetBySpecAsync(new BoardByTitleSpec(title, board.MandantId), ct)
            is not Board existing || existing.Id == board.Id)
                .WithMessage((_, title) => string.Format(localizer["boardTitle.alreadyexists"], title));

        RuleFor(x => x.Description)
            .MaximumLength(100);
        RuleFor(x => x.Color)
            .MaximumLength(15);
        RuleFor(x => x.BoardLabelRemove)
            .MaximumLength(60);
        RuleFor(x => x.DefaultBoardItemLabelGroupId)
            .GreaterThan(0);

    }
}

public class UpdateBoardRequestHandler : IRequestHandler<UpdateBoardRequest, int>
{
    private readonly IRepositoryWithEvents<Board> _repository;
    private readonly IStringLocalizer<UpdateBoardRequestHandler> _localizer;

    public UpdateBoardRequestHandler(IRepositoryWithEvents<Board> repository, IStringLocalizer<UpdateBoardRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardRequest request, CancellationToken cancellationToken)
    {
        var board = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (board is null) throw new NotFoundException(string.Format(_localizer["board.notfound"], request.Id));

        board.Update(request.Title!, request.Description, request.Color!, request.UserOnly, request.BoardLabelAdd, request.BoardLabelRemove, request.DoneBoard, request.DefaultBoardItemLabelGroupId);
        board.DomainEvents.Add(EntityUpdatedEvent.WithEntity(board));
        await _repository.UpdateAsync(board, cancellationToken);
        return board.Id;
    }
}