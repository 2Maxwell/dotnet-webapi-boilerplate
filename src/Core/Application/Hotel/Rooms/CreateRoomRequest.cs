using FluentValidation;
using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public string NameUnique
    {
        get
        {
            return this.Name + "|" + this.MandantId;
        }
    }

    public string PhoneNumberUnique
    {
        get
        {
            return this.PhoneNumber + "|" + this.MandantId;
        }
    }

}

public class CreateRoomRequestValidator : CustomValidator<CreateRoomRequest>
{
    public CreateRoomRequestValidator(IReadRepository<Room> repository, IStringLocalizer<CreateRoomRequestValidator> localizer)
    {

        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50);

        RuleFor(x => x.NameUnique)
        .NotEmpty()
        .MustAsync(async (nameUnique, ct) => await repository.GetBySpecAsync(new RoomByNameSpec(nameUnique), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["roomNameUnique.alreadyexists"], name));

        RuleFor(x => x.PhoneNumber)
        .NotEmpty()
        .MaximumLength(50);

        RuleFor(x => x.PhoneNumberUnique)
        .NotEmpty()
        .MustAsync(async (phoneNumberUnique, ct) => await repository.GetBySpecAsync(new RoomByPhoneNumberSpec(phoneNumberUnique), ct) is null)
        .WithMessage((_, phoneNumber) => string.Format(localizer["roomPhoneNumberUnique.alreadyexists"], phoneNumber));

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
    public RoomByNameSpec(string name) => Query.Where(x => x.Name == name);
}

public class CreateRoomRequestHandler : IRequestHandler<CreateRoomRequest, int>
{
    private readonly IRepository<Room> _repository;

    public CreateRoomRequestHandler(IRepository<Room> repository) => _repository = repository;

    public async Task<int> Handle(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        var room = new Room(request.MandantId, request.CategoryId, request.Name, request.Description, request.DisplayDescription, request.Beds, request.BedsExtra, request.Facilities, request.PhoneNumber);

        room.DomainEvents.Add(EntityCreatedEvent.WithEntity(room));
        await _repository.AddAsync(room, cancellationToken);
        return room.Id;

    }
}

