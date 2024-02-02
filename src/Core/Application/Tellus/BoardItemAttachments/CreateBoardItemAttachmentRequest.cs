using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Tellus.BoardItemAttachments;
public class CreateBoardItemAttachmentRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public int BoardItemId { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string? Path { get; set; }
}

public class CreateBoardItemAttachmentRequestValidator : CustomValidator<CreateBoardItemAttachmentRequest>
{
    public CreateBoardItemAttachmentRequestValidator(IReadRepository<BoardItemAttachment> repository, IStringLocalizer<CreateBoardItemAttachmentRequestValidator> localizer)
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (boardItemAttachment, fileName, ct) =>
                                             await repository.GetBySpecAsync(new BoardItemAttachmentByFileNameSpec(fileName, boardItemAttachment.MandantId), ct)
                                                                                                    is null)
                    .WithMessage((_, fileName) => string.Format(localizer["boardItemAttachmentFileName.alreadyexists"], fileName));

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Path)
            .NotEmpty()
            .MaximumLength(150);
    }
}

public class BoardItemAttachmentByFileNameSpec : Specification<BoardItemAttachment>, ISingleResultSpecification
{
    public BoardItemAttachmentByFileNameSpec(string fileName, int mandantId)
    {
        Query.Where(x => x.FileName == fileName && x.MandantId == mandantId);
    }
}

public class CreateBoardItemAttachmentRequestHandler : IRequestHandler<CreateBoardItemAttachmentRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemAttachment> _repository;
    private readonly IStringLocalizer<CreateBoardItemAttachmentRequestHandler> _localizer;

    public CreateBoardItemAttachmentRequestHandler(IRepositoryWithEvents<BoardItemAttachment> repository, IStringLocalizer<CreateBoardItemAttachmentRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateBoardItemAttachmentRequest request, CancellationToken cancellationToken)
    {
        var boardItemAttachment = new BoardItemAttachment(request.MandantId, request.BoardItemId, request.FileName!, request.ContentType!, request.Path!);
        boardItemAttachment.DomainEvents.Add(EntityCreatedEvent.WithEntity(boardItemAttachment));
        await _repository.AddAsync(boardItemAttachment, cancellationToken);
        return boardItemAttachment.Id;
    }
}