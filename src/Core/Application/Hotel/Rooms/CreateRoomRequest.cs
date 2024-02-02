using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;

public class CreateRoomRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? DisplayDescription { get; set; }
    public int Beds { get; set; } = 1;
    public int BedsExtra { get; set; }
    public string? Facilities { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Clean { get; set; }
    public bool Blocked { get; set; }
    public DateTime? BlockedStart { get; set; }
    public DateTime? BlockedEnd { get; set; }
    public int CleaningState { get; set; }
    public int DirtyDays { get; set; }
    public int AssignedId { get; set; }
    public int MinutesOccupied { get; set; }
    public int MinutesDeparture { get; set; }
    public int MinutesDefault { get; set; }

}

public class CreateRoomRequestValidator : CustomValidator<CreateRoomRequest>
{
    public CreateRoomRequestValidator(IReadRepository<Room> repository, IStringLocalizer<CreateRoomRequestValidator> localizer)
    {

        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50)
        .MustAsync(async (room, name, ct) =>
        await repository.GetBySpecAsync(new RoomByNameSpec(name, room.MandantId), ct)
        is not Room existingRoom || existingRoom.MandantId == room.MandantId)
        .WithMessage((_, name) => string.Format(localizer["roomName.alreadyexists"], name));

        RuleFor(x => x.PhoneNumber)
        .NotEmpty()
        .MaximumLength(50)
                .MustAsync(async (room, phoneNumber, ct) =>
        await repository.GetBySpecAsync(new RoomByNameSpec(phoneNumber, room.MandantId), ct)
        is not Room existingRoom || existingRoom.MandantId == room.MandantId)
        .WithMessage((_, phoneNumber) => string.Format(localizer["roomPhoneNumber.alreadyexists"], phoneNumber));

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(100);

        RuleFor(x => x.Facilities)
            .MaximumLength(500);

        RuleFor(x => x.DisplayDescription)
            .MaximumLength(500);

        RuleFor(x => x.MandantId)
            .NotEmpty();
    }
}

public class RoomByPhoneNumberSpec : Specification<Room>, ISingleResultSpecification
{
    public RoomByPhoneNumberSpec(string phoneNumber) => Query.Where(x => x.PhoneNumber == phoneNumber);
}

public class RoomByNameSpec : Specification<Room>, ISingleResultSpecification
{
    public RoomByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreateRoomRequestHandler : IRequestHandler<CreateRoomRequest, int>
{
    private readonly IRepository<Room> _repository;

    public CreateRoomRequestHandler(IRepository<Room> repository) => _repository = repository;

    public async Task<int> Handle(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        var room = new Room(request.MandantId, request.CategoryId, request.Name, request.Description, request.DisplayDescription, request.Beds, request.BedsExtra, request.Facilities, request.PhoneNumber, request.CleaningState, request.DirtyDays,
            request.AssignedId, request.MinutesOccupied, request.MinutesDeparture, request.MinutesDefault);

        room.DomainEvents.Add(EntityCreatedEvent.WithEntity(room));
        await _repository.AddAsync(room, cancellationToken);
        return room.Id;

    }
}

