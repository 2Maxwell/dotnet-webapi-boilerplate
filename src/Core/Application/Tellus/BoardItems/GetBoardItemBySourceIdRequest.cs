using FSH.WebApi.Domain.Boards;
using Mapster;
using Newtonsoft.Json;

namespace FSH.WebApi.Application.Tellus.BoardItems;
public class GetBoardItemBySourceIdRequest : IRequest<List<BoardItemDto>>
{
    public int MandantId { get; set; }
    public string? Source { get; set; }
    public int SourceId { get; set; }
    public GetBoardItemBySourceIdRequest(int mandantId, string? source, int sourceId) => (MandantId, Source, SourceId) = (mandantId, source, sourceId);
}

public class GetBoardItemBySourceIdRequestHandler : IRequestHandler<GetBoardItemBySourceIdRequest, List<BoardItemDto>>
{
    private readonly IRepository<BoardItem> _repository;
    private readonly IStringLocalizer<GetBoardItemBySourceIdRequestHandler> _localizer;

    public GetBoardItemBySourceIdRequestHandler(IRepository<BoardItem> repository, IStringLocalizer<GetBoardItemBySourceIdRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<BoardItemDto>> Handle(GetBoardItemBySourceIdRequest request, CancellationToken cancellationToken)
    {
        var boardItems = (await _repository.ListAsync(cancellationToken))
            .Where(x => x.MandantId == request.MandantId && x.BoardSourceIdJson! is not null && x.BoardSourceIdJson.Contains($"{request.Source}|{request.SourceId}"))
            .OrderBy(x => x.Start)
            .ToList().Adapt<List<BoardItemDto>>();

        // BoardItemRepeaterJson is a stringified JSON object, so we need to deserialize it
        boardItems.ForEach(x => x.BoardItemRepeater = JsonConvert.DeserializeObject<BoardItemRepeater>(x.BoardItemRepeaterJson ?? string.Empty));

        // BoardSourceIdJson is a stringified JSON object, so we need to deserialize it
        boardItems.ForEach(x => x.BoardSourceIds = JsonConvert.DeserializeObject<List<BoardSourceId>>(x.BoardSourceIdJson ?? string.Empty));

        return boardItems;  // .Adapt<List<BoardItemDto>>();
    }
}
