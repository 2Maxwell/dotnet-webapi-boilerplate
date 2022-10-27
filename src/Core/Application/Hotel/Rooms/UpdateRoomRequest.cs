using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;

public class UpdateRoomRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? DisplayDescription { get; set; }
    public int Beds { get; set; } = 1;
    public int BedsExtra { get; set; }
    public string? Facilities { get; set; }
    public bool Clean { get; set; }
    public bool Blocked { get; set; }
    public DateTime? BlockedStart { get; set; }
    public DateTime? BlockedEnd { get; set; }
    public string? PhoneNumber { get; set; }
}

public class UpdateRoomRequestValidator : CustomValidator<UpdateRoomRequest>
{
    public UpdateRoomRequestValidator(IReadRepository<Room> repository, IStringLocalizer<UpdateRoomRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50);

            // .MustAsync(async (room, nameUnique, ct) =>
            //      await repository.GetBySpecAsync(new RoomByNameSpec(nameUnique), ct)
            //      is not Room existingRoom || existingRoom.Id == room.Id)
            //      .WithMessage((_, name) => string.Format(localizer["roomName.alreadyexists"], name));

        RuleFor(x => x.PhoneNumber)
        .NotEmpty()
        .MaximumLength(50);

            // .MustAsync(async (room, phoneNumber, ct) =>
            //        await repository.GetBySpecAsync(new RoomByPhoneNumberSpec(phoneNumber), ct)
            //        is not Room existingRoom || existingRoom.PhoneNumber == room.PhoneNumber)
            //        .WithMessage((_, phoneNumber) => string.Format(localizer["roomPhoneNumber.alreadyexists"], phoneNumber));

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(100);

        RuleFor(x => x.Facilities)
            .MaximumLength(500);

        RuleFor(x => x.DisplayDescription)
            .MaximumLength(500);
    }
}

public class UpdateRoomRequestHandler : IRequestHandler<UpdateRoomRequest, int>
{
    private readonly IRepositoryWithEvents<Room> _repository;
    private readonly IStringLocalizer<UpdateRoomRequestHandler> _localizer;

    public UpdateRoomRequestHandler(IRepositoryWithEvents<Room> repository, IStringLocalizer<UpdateRoomRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateRoomRequest request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = room ?? throw new NotFoundException(string.Format(_localizer["room.notfound"], request.Id));

        room.Update(
            request.CategoryId,
            request.Name,
            request.Description,
            request.DisplayDescription,
            request.Beds,
            request.BedsExtra,
            request.Facilities,
            request.PhoneNumber,
            request.Clean,
            request.Blocked,
            request.BlockedStart,
            request.BlockedEnd);

        await _repository.UpdateAsync(room, cancellationToken);

        return request.Id;
    }

}