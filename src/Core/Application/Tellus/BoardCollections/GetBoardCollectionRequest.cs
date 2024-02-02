using FSH.WebApi.Application.Tellus.BoardItemLabels;
using FSH.WebApi.Application.Tellus.Boards;
using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardCollections;
public class GetBoardCollectionRequest : IRequest<BoardCollectionDto>
{
    public int Id { get; set; }
    public GetBoardCollectionRequest(int id) => Id = id;
}

public class GetBoardCollectionRequestHandler : IRequestHandler<GetBoardCollectionRequest, BoardCollectionDto>
{
    private readonly IRepository<BoardCollection> _repository;
    private readonly IRepository<Board> _boardRepository;
    private readonly IRepository<BoardItemLabel> _boardItemLabelRepository;
    private readonly IStringLocalizer<GetBoardCollectionRequestHandler> _localizer;

    public GetBoardCollectionRequestHandler(IRepository<BoardCollection> repository, IRepository<Board> boardRepository, IRepository<BoardItemLabel> boardItemLabelRepository, IStringLocalizer<GetBoardCollectionRequestHandler> localizer)
    {
        _repository = repository;
        _boardRepository = boardRepository;
        _boardItemLabelRepository = boardItemLabelRepository;
        _localizer = localizer;
    }

    public async Task<BoardCollectionDto> Handle(GetBoardCollectionRequest request, CancellationToken cancellationToken)
    {
        var boardCollection = (await _repository.GetByIdAsync(request.Id, cancellationToken))!.Adapt<BoardCollectionDto>()
        ?? throw new NotFoundException(string.Format(_localizer["boardcollection.notfound"], request.Id));

        if (!string.IsNullOrEmpty(boardCollection.BoardItemLabelIds))
        {
            string[] boardItemLabelIds = boardCollection.BoardItemLabelIds.Split('|');
            boardCollection.BoardItemLabels = new List<BoardItemLabelDto>();
            foreach (string boardItemLabelId in boardItemLabelIds)
            {
                int number;
                bool success = Int32.TryParse(boardItemLabelId, out number);
                if (success)
                {
                    var boardItemLabel = await _boardItemLabelRepository.GetByIdAsync(number, cancellationToken);
                    if (boardItemLabel is null) throw new NotFoundException(string.Format(_localizer["boarditemlabel.notfound"], number));
                    boardCollection.BoardItemLabels!.Add(boardItemLabel.Adapt<BoardItemLabelDto>());
                }
            }

            //var boardItemLabelIds = boardCollection.BoardItemLabelIds.Split('|').Select(int.Parse).ToList();
            //foreach (int boardItemLabelId in boardItemLabelIds)
            //{
            //    var boardItemLabel = await _boardItemLabelRepository.GetByIdAsync(boardItemLabelId, cancellationToken);
            //    if (boardItemLabel is null) throw new NotFoundException(string.Format(_localizer["boarditemlabel.notfound"], boardItemLabelId));
            //    boardCollection.BoardItemLabels!.Add(boardItemLabel.Adapt<BoardItemLabelDto>());
            //}
        }

        if (!string.IsNullOrEmpty(boardCollection.BoardIds))
        {

            string[] boardIds = boardCollection.BoardIds.Split('|');
            boardCollection.Boards = new List<BoardDto>();
            foreach (string boardId in boardIds)
            {
                int number;
                bool success = Int32.TryParse(boardId, out number);
                if (success)
                {
                    var board = await _boardRepository.GetByIdAsync(number, cancellationToken);
                    if (board is null) throw new NotFoundException(string.Format(_localizer["board.notfound"], number));
                    boardCollection.Boards!.Add(board.Adapt<BoardDto>());
                }
            }

            //var boardIds = boardCollection.BoardIds.Split('|').Select(int.Parse).ToList();
            //foreach (int boardId in boardIds)
            //{
            //    var board = await _boardRepository.GetByIdAsync(boardId, cancellationToken);
            //    if (board is null) throw new NotFoundException(string.Format(_localizer["board.notfound"], boardId));
            //    boardCollection.Boards!.Add(board.Adapt<BoardDto>());
            //}
        }

        return boardCollection;
    }
}
