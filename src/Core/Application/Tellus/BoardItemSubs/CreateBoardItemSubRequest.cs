using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Tellus.BoardItemSubs;
public class CreateBoardItemSubRequest : IRequest<int>
{
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

public class CreateBoardItemSubRequestValidator : CustomValidator<CreateBoardItemSubRequest>
{
    public CreateBoardItemSubRequestValidator(IReadRepository<BoardItemSub> repository, IStringLocalizer<CreateBoardItemSubRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.BoardItemId)
            .NotEmpty();

        RuleFor(x => x.OrderNumber)
            .NotEmpty();

        RuleFor(x => x.ResultType)
            .MaximumLength(20);

        RuleFor(x => x.ResultLabel)
            .MaximumLength(40);

        RuleFor(x => x.ResultValueString)
            .MaximumLength(250);
    }
}

//public class BoardItemSubByTitleSpec : Specification<BoardItemSub>, ISingleResultSpecification
//{
//    public BoardItemSubByTitleSpec(string title, int mandantId)
//    {
//        Query.Where(x => x.Title == title && x.MandantId == mandantId);
//    }
//}

public class CreateBoardItemSubRequestHandler : IRequestHandler<CreateBoardItemSubRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemSub> _repository;
    private readonly IStringLocalizer<CreateBoardItemSubRequestHandler> _localizer;

    public CreateBoardItemSubRequestHandler(IRepositoryWithEvents<BoardItemSub> repository, IStringLocalizer<CreateBoardItemSubRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardItemSubRequest request, CancellationToken cancellationToken)
    {
        var boardItemSub = new BoardItemSub(request.MandantId, request.BoardItemId, request.OrderNumber, request.Title!, request.Text, request.ResultType, request.ResultLabel, request.ResultValueInt, request.ResultValueDecimal, request.ResultValueString, request.ResultValueBool);
        boardItemSub.DomainEvents.Add(EntityCreatedEvent.WithEntity(boardItemSub));
        await _repository.AddAsync(boardItemSub, cancellationToken);
        return boardItemSub.Id;
    }
}