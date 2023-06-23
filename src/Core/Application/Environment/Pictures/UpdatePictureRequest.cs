using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Environment.Pictures;
public class UpdatePictureRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int OrderNumber { get; set; }
    public string? MatchCode { get; set; }
    public string? ImagePath { get; set; }
    public bool ShowPicture { get; set; } = true;
    public bool Publish { get; set; } = true;
    public bool DiaShow { get; set; } = true;
    public bool DeleteCurrentImage { get; set; } = false;
    public FileUploadRequest? Image { get; set; }
}

public class UpdatePictureRequestValidator : CustomValidator<UpdatePictureRequest>
{
    public UpdatePictureRequestValidator(IReadRepository<Picture> repository, IStringLocalizer<UpdatePictureRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (picture, title, ct) =>
                await repository.GetBySpecAsync(new PictureByTitleSpec(title, picture.MandantId), ct)
                is not Picture existing || existing.Id == picture.Id)
                .WithMessage((_, name) => string.Format(localizer["packageName.alreadyexists"], name));
        RuleFor(x => x.Description)
            .MaximumLength(100);
        RuleFor(x => x.OrderNumber)
            .GreaterThan(0);
    }
}

public class UpdatePictureRequestHandler : IRequestHandler<UpdatePictureRequest, int>
{
    private readonly IRepository<Picture> _repository;
    private readonly IStringLocalizer _t;
    private readonly IFileStorageService _file;

    public UpdatePictureRequestHandler(IRepository<Picture> repository, IStringLocalizer<UpdatePictureRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _t, _file) = (repository, localizer, file);

    public async Task<int> Handle(UpdatePictureRequest request, CancellationToken cancellationToken)
    {
        var picture = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = picture ?? throw new NotFoundException(_t["Picture {0} Not Found.", request.Id]);

        // Remove old image if flag is set
        if (request.DeleteCurrentImage)
        {
            string? currentPictureImagePath = picture.ImagePath;
            if (!string.IsNullOrEmpty(currentPictureImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentPictureImagePath));
            }

            picture = picture.ClearImagePath();
        }

        string? pictureImagePath = request.Image is not null
            ? await _file.UploadAsync<Picture>(request.Image, FileType.Image, cancellationToken)
            : null;

        var updatedPicture = picture.Update(
            request.Title,
            request.Description,
            request.OrderNumber,
            request.MatchCode,
            pictureImagePath,
            request.ShowPicture,
            request.Publish,
            request.DiaShow
            );

        // Add Domain Events to be raised after the commit
        picture.DomainEvents.Add(EntityUpdatedEvent.WithEntity(picture));

        await _repository.UpdateAsync(updatedPicture, cancellationToken);

        return request.Id;
    }
}