using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.Boards;
public class GetBoardRequest : IRequest<BoardDto>
{
    public int Id { get; set; }
    public GetBoardRequest(int id) => Id = id;
}

public class GetBoardRequestHandler : IRequestHandler<GetBoardRequest, BoardDto>
{
    private readonly IRepository<Board> _repository;
    private readonly IStringLocalizer<GetBoardRequestHandler> _localizer;

    public GetBoardRequestHandler(IRepository<Board> repository, IStringLocalizer<GetBoardRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<BoardDto> Handle(GetBoardRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync((ISpecification<Board, BoardDto>)new BoardByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["board.notfound"], request.Id));
}
