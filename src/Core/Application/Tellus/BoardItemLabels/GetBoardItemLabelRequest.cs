using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardItemLabels;
public class GetBoardItemLabelRequest : IRequest<BoardItemLabelDto>
{
    public int Id { get; set; }
    public GetBoardItemLabelRequest(int id) => Id = id;
}

public class GetBoardItemLabelRequestHandler : IRequestHandler<GetBoardItemLabelRequest, BoardItemLabelDto>
{
    private readonly IRepository<BoardItemLabel> _repository;
    private readonly IStringLocalizer<GetBoardItemLabelRequest> _localizer;

    public GetBoardItemLabelRequestHandler(IRepository<BoardItemLabel> repository, IStringLocalizer<GetBoardItemLabelRequest> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<BoardItemLabelDto> Handle(GetBoardItemLabelRequest request, CancellationToken cancellationToken)
    {
        var boardItemLabel = (await _repository.GetByIdAsync(request.Id, cancellationToken))!.Adapt<BoardItemLabelDto>()
        ?? throw new NotFoundException(string.Format(_localizer["boarditemlabel.notfound"], request.Id));

        return boardItemLabel;
    }
}
