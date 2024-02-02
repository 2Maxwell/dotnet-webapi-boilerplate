using FSH.WebApi.Application.Tellus.BoardItemLabels;
using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.BoardItemLabelGroups;
public class GetBoardItemLabelGroupsRequest : IRequest<List<BoardItemLabelGroupDto>>
{
    public int MandantId { get; set; }
    public GetBoardItemLabelGroupsRequest(int mandantId)
    {
        MandantId = mandantId;
    }
}

public class GetBoardItemLabelGroupsRequestHandler : IRequestHandler<GetBoardItemLabelGroupsRequest, List<BoardItemLabelGroupDto>>
{
    private readonly IRepository<BoardItemLabelGroup> _repository;
    private readonly IRepository<BoardItemLabel> _repositoryBoardItemLabel;
    private readonly IStringLocalizer<GetBoardItemLabelGroupsRequestHandler> _localizer;

    public GetBoardItemLabelGroupsRequestHandler(IRepository<BoardItemLabelGroup> repository, IRepository<BoardItemLabel> repositoryBoardItemLabel, IStringLocalizer<GetBoardItemLabelGroupsRequestHandler> localizer)
    {
        _repository = repository;
        _repositoryBoardItemLabel = repositoryBoardItemLabel;
        _localizer = localizer;
    }

    public async Task<List<BoardItemLabelGroupDto>> Handle(GetBoardItemLabelGroupsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemLabelGroupSpecification(request.MandantId);
        var boardItemLabelGroups = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemLabelGroups is null) throw new NotFoundException(_localizer["boarditemlabelgroup.notfound"]);

        // BoardItemLabelGroupDto.BoardItemLabels mit BoardItemLabels füllen

        foreach (BoardItemLabelGroupDto item in boardItemLabelGroups)
        {
            var spec2 = new BoardItemLabelSpecification(item.Id);
            var boardItemLabels = await _repositoryBoardItemLabel.ListAsync(spec2, cancellationToken);
            if (boardItemLabels is null) throw new NotFoundException(_localizer["boarditemlabel.notfound"]);

            item.BoardItemLabels = boardItemLabels;
        }

        return boardItemLabelGroups;
    }
}

public class BoardItemLabelSpecification : Specification<BoardItemLabel, BoardItemLabelDto>
{
    public BoardItemLabelSpecification(int boardItemLabelGroupId) =>
        Query.Where(x => x.BoardItemLabelGroupId == boardItemLabelGroupId);
}

public class BoardItemLabelGroupSpecification : Specification<BoardItemLabelGroup, BoardItemLabelGroupDto>
{
    public BoardItemLabelGroupSpecification(int mandantId) =>
        Query.Where(x => x.MandantId == mandantId);
}