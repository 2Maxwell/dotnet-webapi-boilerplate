using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Pictures;
public class CreatePictureRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int OrderNumber { get; set; }
    public string? MatchCode { get; set; }

    // public string? ImagePath { get; set; }

    public bool ShowPicture { get; set; } = true;
    public bool Publish { get; set; } = true;
    public bool DiaShow { get; set; } = true;
    public FileUploadRequest? Image { get; set; }

}

public class CreatePictureRequestValidator : CustomValidator<CreatePictureRequest>
{
    public CreatePictureRequestValidator(IReadRepository<Picture> repository, IStringLocalizer<CreatePictureRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (picture, title, ct) => await repository.GetBySpecAsync(new PictureByTitleSpec(title, picture.MandantId), ct) is null)
            .WithMessage((_, title) => string.Format(localizer["title.alreadyexists"], title));
        RuleFor(x => x.Description)
            .MaximumLength(100);

        // RuleFor(x => x.MatchCode)
        //    .NotEmpty()
        //    .MaximumLength(500);

        RuleFor(x => x.OrderNumber)
            .GreaterThan(0);
    }
}

public class PictureByTitleSpec : Specification<Picture>, ISingleResultSpecification
{
    public PictureByTitleSpec(string title, int mandantId) =>
        Query.Where(x => x.Title == title && x.MandantId == mandantId);
}

public class CreatePictureRequestHandler : IRequestHandler<CreatePictureRequest, int>
{
    private readonly IRepository<Picture> _repository;
    private readonly IFileStorageService _file;

    public CreatePictureRequestHandler(IRepository<Picture> repository, IFileStorageService file)
    {
        _repository = repository;
        _file = file;
    }

    public async Task<int> Handle(CreatePictureRequest request, CancellationToken cancellationToken)
    {
        string pictureImagePath = await _file.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken);

        var picture = new Picture(
            request.MandantId,
            request.Title,
            request.Description,
            request.OrderNumber,
            request.MatchCode,
            pictureImagePath,
            request.ShowPicture,
            request.Publish,
            request.DiaShow);
        picture.DomainEvents.Add(EntityCreatedEvent.WithEntity(picture));
        await _repository.AddAsync(picture, cancellationToken);

        return picture.Id;
    }
}