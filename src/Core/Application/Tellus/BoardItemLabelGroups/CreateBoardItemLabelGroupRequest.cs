using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Tellus.BoardItemLabelGroups;
public class CreateBoardItemLabelGroupRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string Title { get; set; } = null!;
}

public class CreateBoardItemLabelGroupRequestValidator : CustomValidator<CreateBoardItemLabelGroupRequest>
{
    public CreateBoardItemLabelGroupRequestValidator(IReadRepository<BoardItemLabelGroup> repository, IStringLocalizer<CreateBoardItemLabelGroupRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
        .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(25)
            .MustAsync(async (boardItemLabelGroup, title, ct) =>
                                             await repository.GetBySpecAsync(new BoardItemLabelGroupByTitleSpec(title, boardItemLabelGroup.MandantId), ct)
                                                                                                    is null)
                    .WithMessage((_, title) => string.Format(localizer["boardItemLabelGroupTitle.alreadyexists"], title));
    }
}

public class BoardItemLabelGroupByTitleSpec : Specification<BoardItemLabelGroup>, ISingleResultSpecification
{
    public BoardItemLabelGroupByTitleSpec(string title, int mandantId)
    {
        Query.Where(x => x.Title == title && x.MandantId == mandantId);
    }
}

public class CreateBoardItemLabelGroupRequestHandler : IRequestHandler<CreateBoardItemLabelGroupRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemLabelGroup> _repository;
    private readonly IStringLocalizer<CreateBoardItemLabelGroupRequestHandler> _localizer;

    public CreateBoardItemLabelGroupRequestHandler(IRepositoryWithEvents<BoardItemLabelGroup> repository, IStringLocalizer<CreateBoardItemLabelGroupRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardItemLabelGroupRequest request, CancellationToken cancellationToken)
    {
        var boardItemLabelGroup = new BoardItemLabelGroup(request.MandantId, request.Title!);
        boardItemLabelGroup.DomainEvents.Add(EntityCreatedEvent.WithEntity(boardItemLabelGroup));
        await _repository.AddAsync(boardItemLabelGroup, cancellationToken);
        return boardItemLabelGroup.Id;
    }
}