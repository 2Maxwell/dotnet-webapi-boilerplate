using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class GetCategoryByKzRequest : IRequest<CategoryDto>
{
    public string Kz { get; set; }
    public int MandantId { get; set; }
    public GetCategoryByKzRequest(string kz, int mandantId) => (Kz, MandantId) = (kz, mandantId);
}

public class GetCategoryByKzRequestHandler : IRequestHandler<GetCategoryByKzRequest, CategoryDto>
{
    private readonly IRepository<Category> _repository;
    private readonly IStringLocalizer<GetCategoryByKzRequestHandler> _localizer;

    public GetCategoryByKzRequestHandler(IRepository<Category> repository, IStringLocalizer<GetCategoryByKzRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<CategoryDto> Handle(GetCategoryByKzRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Category, CategoryDto>)new CategoryByKzSpec(request.Kz, request.MandantId), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["category.notfound"], request.Kz));
}
