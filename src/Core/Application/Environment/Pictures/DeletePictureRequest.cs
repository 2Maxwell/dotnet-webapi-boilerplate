using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Pictures;
public class DeletePictureRequest : IRequest<int>
{
    public int Id { get; set; }
    public DeletePictureRequest(int id)
    {
        Id = id;
    }
}

public class DeletePictureRequestHandler : IRequestHandler<DeletePictureRequest, int>
{
    private readonly IRepository<Picture> _repository;
    private readonly IStringLocalizer _t;

    public DeletePictureRequestHandler(IRepository<Picture> repository, IStringLocalizer<DeletePictureRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<int> Handle(DeletePictureRequest request, CancellationToken cancellationToken)
    {
        var picture = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = picture ?? throw new NotFoundException(_t["Picture {0} Not Found."]);

        // Add Domain Events to be raised after the commit
        picture.DomainEvents.Add(EntityDeletedEvent.WithEntity(picture));

        await _repository.DeleteAsync(picture, cancellationToken);

        return request.Id;
    }
}
