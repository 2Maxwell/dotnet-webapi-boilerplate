using FSH.WebApi.Domain.Boards;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Tellus.BoardItemAttachments;
public class UpdateBoardItemAttachmentRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int BoardItemId { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string? Path { get; set; }
}

public class UpdateBoardItemAttachmentRequestValidator : CustomValidator<UpdateBoardItemAttachmentRequest>
{
    public UpdateBoardItemAttachmentRequestValidator(IReadRepository<BoardItemAttachment> repository, IStringLocalizer<UpdateBoardItemAttachmentRequestValidator> localizer)
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (boardItemAttachment, fileName, ct) =>
                                                             await repository.GetBySpecAsync(new BoardItemAttachmentByFileNameSpec(fileName, boardItemAttachment.MandantId), ct)
                                                             is not BoardItemAttachment existing || existing.Id == boardItemAttachment.Id)
                    .WithMessage((_, fileName) => string.Format(localizer["boardItemAttachmentFileName.alreadyexists"], fileName));

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Path)
            .NotEmpty()
            .MaximumLength(150);
    }
}

public class UpdateBoardItemAttachmentRequestHandler : IRequestHandler<UpdateBoardItemAttachmentRequest, int>
{
    private readonly IRepositoryWithEvents<BoardItemAttachment> _repository;
    private readonly IStringLocalizer<UpdateBoardItemAttachmentRequestHandler> _localizer;

    public UpdateBoardItemAttachmentRequestHandler(IRepositoryWithEvents<BoardItemAttachment> repository, IStringLocalizer<UpdateBoardItemAttachmentRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateBoardItemAttachmentRequest request, CancellationToken cancellationToken)
    {
        var boardItemAttachment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (boardItemAttachment is null) throw new NotFoundException(string.Format(_localizer["boardItemAttachment.notfound"], request.Id));

        boardItemAttachment.Update(request.ContentType);
        boardItemAttachment.DomainEvents.Add(EntityUpdatedEvent.WithEntity(boardItemAttachment));
        await _repository.UpdateAsync(boardItemAttachment, cancellationToken);
        return boardItemAttachment.Id;
    }
}