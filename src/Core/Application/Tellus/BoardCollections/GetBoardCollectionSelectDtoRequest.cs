using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.BoardCollections;
public class GetBoardCollectionSelectDtoRequest : IRequest<List<BoardCollectionSelectDto>>
{
    public int MandantId { get; set; }
    public string? UserId { get; set; }
}

public class GetBoardCollectionSelectDtoRequestHandler : IRequestHandler<GetBoardCollectionSelectDtoRequest, List<BoardCollectionSelectDto>>
{
    private readonly IReadRepository<BoardCollection> _repository;

    public GetBoardCollectionSelectDtoRequestHandler(IReadRepository<BoardCollection> repository)
    {
        _repository = repository;
    }

    public async Task<List<BoardCollectionSelectDto>> Handle(GetBoardCollectionSelectDtoRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardCollectionsByMandantUserSearchRequestSpec(request.MandantId, request.UserId!);
        var boardCollections = await _repository.ListAsync(spec, cancellationToken);

        // var boardCollections = await _repository.GetAllAsync(bc => bc.MandantId == request.MandantId && (bc.UserOnly is false || bc.CreatedBy == request.UserId), cancellationToken);
        // return _mapper.Map<List<BoardCollectionSelectDto>>(boardCollections);

        return boardCollections;
    }
}

public class BoardCollectionsByMandantUserSearchRequestSpec : Specification<BoardCollection, BoardCollectionSelectDto>
{
    public BoardCollectionsByMandantUserSearchRequestSpec(int mandantId, string userId)
    {
        Query.Where(x => x.MandantId == mandantId && (!x.UserOnly || x.CreatedBy == new Guid(userId!)));
    }
}