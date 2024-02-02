using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Tellus.Enums;
public class GetBoardItemTypeEnumRequest : IRequest<List<BoardItemTypeEnumDto>>
{
}

public class GetBoardItemTypeEnumHandler : IRequestHandler<GetBoardItemTypeEnumRequest, List<BoardItemTypeEnumDto>>
{
    private readonly IStringLocalizer<GetBoardItemTypeEnumHandler> _localizer;
    public GetBoardItemTypeEnumHandler(IStringLocalizer<GetBoardItemTypeEnumHandler> localizer) => _localizer = localizer;
    public Task<List<BoardItemTypeEnumDto>> Handle(GetBoardItemTypeEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(BoardItemTypeEnum)).Cast<BoardItemTypeEnum>().Select(e => new BoardItemTypeEnumDto
        {
            Name = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class BoardItemTypeEnumDto : IDto
{
    public int Value { get; set; }
    public string Name { get; set; }
}