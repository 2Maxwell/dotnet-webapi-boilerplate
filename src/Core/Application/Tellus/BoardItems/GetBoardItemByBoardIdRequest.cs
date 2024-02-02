using FSH.WebApi.Application.Tellus.BoardItemSubs;
using FSH.WebApi.Domain.Boards;
using Mapster;
using Newtonsoft.Json;

namespace FSH.WebApi.Application.Tellus.BoardItems;
public class GetBoardItemByBoardIdRequest : IRequest<List<BoardItemDto>>
{
    public int BoardId { get; set; }
    public int MandantId { get; set; }
    public GetBoardItemByBoardIdRequest(int boardId, int mandantId) => (BoardId, MandantId) = (boardId, mandantId);
}

public class GetBoardItemByBoardIdRequestHandler : IRequestHandler<GetBoardItemByBoardIdRequest, List<BoardItemDto>>
{
    private readonly IRepository<BoardItem> _repository;
    private readonly IStringLocalizer<GetBoardItemByBoardIdRequestHandler> _localizer;
    private readonly IRepository<BoardItemLabel> _boardItemLabelRepository;
    private readonly IRepository<BoardItemTag> _boardItemTagRepository;
    private readonly IRepository<BoardItemSub> _boardItemSubRepository;

    public GetBoardItemByBoardIdRequestHandler(IRepository<BoardItem> repository, IStringLocalizer<GetBoardItemByBoardIdRequestHandler> localizer, IRepository<BoardItemLabel> boardItemLabelRepository, IRepository<BoardItemTag> boardItemTagRepository, IRepository<BoardItemSub> boardItemSubRepository)
    {
        _repository = repository;
        _localizer = localizer;
        _boardItemLabelRepository = boardItemLabelRepository;
        _boardItemTagRepository = boardItemTagRepository;
        _boardItemSubRepository = boardItemSubRepository;
    }

    public async Task<List<BoardItemDto>> Handle(GetBoardItemByBoardIdRequest request, CancellationToken cancellationToken)
    {
        var boardItems = (await _repository.ListAsync(cancellationToken))
            .Where(x => x.BoardId == request.BoardId && x.MandantId == request.MandantId)
            .OrderBy(x => x.LastModifiedOn)
            .ToList().Adapt<List<BoardItemDto>>();

        // BoardItemRepeaterJson is a stringified JSON object, so we need to deserialize it
        boardItems.ForEach(x => x.BoardItemRepeater = JsonConvert.DeserializeObject<BoardItemRepeater>(x.BoardItemRepeaterJson ?? string.Empty));

        // BoardSourceIdJson is a stringified JSON object, so we need to deserialize it
        boardItems.ForEach(x => x.BoardSourceIds = JsonConvert.DeserializeObject<List<BoardSourceId>>(x.BoardSourceIdJson ?? string.Empty));

        foreach (var item in boardItems)
        {
            item.ItemSubs = (await _boardItemSubRepository.ListAsync(new BoardItemSubByBoardItemIdSpecification(item.MandantId, item.Id), cancellationToken)).Adapt<List<BoardItemSubDto>>();
        }

        // die folgenden Funktionen werden im Client erledigt. Hier nur zum Verständnis

        //foreach (var boardItem in boardItems)
        //{
        //    if (boardItem.BoardItemLabelIds is not null && boardItem.BoardItemLabelIds.EndsWith("|")) boardItem.BoardItemLabelIds = boardItem.BoardItemLabelIds.Remove(boardItem.BoardItemLabelIds.Length - 1);

        //    List<int> labelIds = boardItem.BoardItemLabelIds?.Split('|').Select(int.Parse).ToList() ?? new List<int>();
        //    foreach (var labelId in labelIds)
        //    {
        //        var label = await _boardItemLabelRepository.GetByIdAsync(labelId, cancellationToken);
        //        boardItem.ItemLabels.Add(label.Adapt<BoardItemLabelDto>());
        //    }

        //    if (boardItem.BoardItemTagIds is not null && boardItem.BoardItemTagIds.EndsWith("|")) boardItem.BoardItemTagIds = boardItem.BoardItemTagIds.Remove(boardItem.BoardItemTagIds.Length - 1);
        //    List<int> tagIds = boardItem.BoardItemTagIds?.Split('|').Select(int.Parse).ToList() ?? new List<int>();
        //    foreach (var tagId in tagIds)
        //    {
        //        var tag = await _boardItemTagRepository.GetByIdAsync(tagId, cancellationToken);
        //        boardItem.ItemTags.Add(tag.Adapt<BoardItemTagDto>());
        //    }
        //}

        return boardItems; //.Adapt<List<BoardItemDto>>();
    }
}

internal class BoardItemSubByBoardItemIdSpecification : Specification<BoardItemSub, BoardItemSubDto>
{
    public BoardItemSubByBoardItemIdSpecification(int mandantId, int boardItemId) =>
        Query.Where(x => x.MandantId == mandantId && x.BoardItemId == boardItemId);
}