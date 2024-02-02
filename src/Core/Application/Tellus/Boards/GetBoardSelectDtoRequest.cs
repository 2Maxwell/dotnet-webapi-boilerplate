using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.Boards;
public class GetBoardSelectDtoRequest : IRequest<List<BoardSelectDto>>
{
    public int MandantId { get; set; }
    public string? UserId { get; set; }
}

public class GetBoardSelectDtoRequestHandler : IRequestHandler<GetBoardSelectDtoRequest, List<BoardSelectDto>>
{
    private readonly IReadRepository<Board> _repository;

    public GetBoardSelectDtoRequestHandler(IReadRepository<Board> repository)
    {
        _repository = repository;
    }

    public async Task<List<BoardSelectDto>> Handle(GetBoardSelectDtoRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardsByMandantUserSearchRequestSpec(request.MandantId, request.UserId!);
        var boards = await _repository.ListAsync(spec, cancellationToken);

        return boards;
    }
}

public class BoardsByMandantUserSearchRequestSpec : Specification<Board, BoardSelectDto>
{
    public BoardsByMandantUserSearchRequestSpec(int mandantId, string userId)
    {
        Query.Where(x => x.MandantId == mandantId && (!x.UserOnly || x.CreatedBy == new Guid(userId!)));
    }
}