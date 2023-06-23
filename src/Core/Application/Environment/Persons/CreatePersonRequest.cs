using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.Persons;
public class CreatePersonRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Source { get; set; } // GuestHotel, GuestRestaurant, CompanyContact, Sharer, MeetingContact
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? Title { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }
    public int CountryId { get; set; }
    public int StateRegionId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Telephone { get; set; }
    public string? Telefax { get; set; }
    public string? Mobil { get; set; }
    public string? Email { get; set; }
    public int? CompanyId { get; set; } = null;
    public int LanguageId { get; set; }
    public int NationalityId { get; set; }
    public bool PersonDelete { get; set; }
    public string? Wishes { get; set; }
    public string? RoomsPreferred { get; set; } // Trenner ist egal, können mehrere Zimmer genannt werden 
    public int SalutationId { get; set; }
    public string? Kz { get; set; } // siehe Source
    public int? StatusId { get; set; }
    public int? VipStatusId { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? Text { get; set; }
}

public class CreatePersonRequestValidator : CustomValidator<CreatePersonRequest>
{
    public CreatePersonRequestValidator(IReadRepository<Person> repository, IStringLocalizer<CreatePersonRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.Source)
            .MaximumLength(150);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.FirstName)
            .MaximumLength(50);
        RuleFor(x => x.Title)
            .MaximumLength(50);
        RuleFor(x => x.Address1)
            .MaximumLength(100);
        RuleFor(x => x.Address2)
            .MaximumLength(100);
        RuleFor(x => x.Zip)
            .MaximumLength(12);
        RuleFor(x => x.City)
            .MaximumLength(70);
        RuleFor(x => x.Telephone)
            .MaximumLength(25);
        RuleFor(x => x.Telefax)
            .MaximumLength(25);
        RuleFor(x => x.Mobil)
            .MaximumLength(25);
        RuleFor(x => x.Email)
            .EmailAddress();
        RuleFor(x => x.Wishes)
            .MaximumLength(250);
        RuleFor(x => x.RoomsPreferred)
            .MaximumLength(50);
        RuleFor(x => x.Kz)
            .MaximumLength(50);
        RuleFor(x => x.Department)
            .MaximumLength(50);
        RuleFor(x => x.Position)
            .MaximumLength(50);
        RuleFor(x => x.Text)
            .MaximumLength(150);
    }
}

public class CreatePersonRequestHandler : IRequestHandler<CreatePersonRequest, int>
{
    private readonly IRepository<Person> _repository;

    public CreatePersonRequestHandler(IRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var person = new Person(request.MandantId,
                                request.Source,
                                request.Name,
                                request.FirstName,
                                request.Title,
                                request.Address1,
                                request.Address2,
                                request.Zip,
                                request.City,
                                request.CountryId,
                                request.StateRegionId,
                                request.DateOfBirth,
                                request.Telephone,
                                request.Telefax,
                                request.Mobil,
                                request.Email,
                                request.CompanyId,
                                request.LanguageId,
                                request.NationalityId,
                                request.PersonDelete,
                                request.Wishes,
                                request.RoomsPreferred,
                                request.SalutationId,
                                request.Kz,
                                request.StatusId,
                                request.VipStatusId,
                                request.Department,
                                request.Position,
                                request.Text);
        person.DomainEvents.Add(EntityCreatedEvent.WithEntity(person));
        await _repository.AddAsync(person,cancellationToken);
        return person.Id;
    }
}