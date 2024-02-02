using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Tellus.BoardItemSubs;
public class UpdateBoardItemSubRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int BoardItemId { get; set; }
    public int OrderNumber { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
    public string? ResultType { get; set; }
    public string? ResultLabel { get; set; }
    public int? ResultValueInt { get; set; }
    public decimal? ResultValueDecimal { get; set; }
    public string? ResultValueString { get; set; }
    public bool? ResultValueBool { get; set; }
}

public class UpdateBoardItemSubRequestValidator : AbstractValidator<UpdateBoardItemSubRequest>
{
    public UpdateBoardItemSubRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.MandantId).NotEmpty();
        RuleFor(x => x.BoardItemId).NotEmpty();
        RuleFor(x => x.OrderNumber).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Text).MaximumLength(250);
        RuleFor(x => x.ResultType).MaximumLength(20);
        RuleFor(x => x.ResultLabel).MaximumLength(40);
        RuleFor(x => x.ResultValueString).MaximumLength(250);
    }
}

public class UpdateBoardItemSubRequestHandler : IRequestHandler<UpdateBoardItemSubRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemSub> _repository;
    private readonly IStringLocalizer<UpdateBoardItemSubRequestHandler> _localizer;

    public UpdateBoardItemSubRequestHandler(IRepositoryWithEvents<BoardItemSub> repository, IStringLocalizer<UpdateBoardItemSubRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardItemSubRequest request, CancellationToken cancellationToken)
    {
        var boardItemSub = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (boardItemSub is null) throw new NotFoundException(string.Format(_localizer["BoardItemSub.notfound"], request.Id));

        boardItemSub.Update(request.OrderNumber, request.Title, request.Text, request.ResultType, request.ResultLabel, request.ResultValueInt, request.ResultValueDecimal, request.ResultValueString, request.ResultValueBool);
        boardItemSub.DomainEvents.Add(EntityUpdatedEvent.WithEntity(boardItemSub));
        await _repository.UpdateAsync(boardItemSub, cancellationToken);
        return boardItemSub.Id;
    }
}
