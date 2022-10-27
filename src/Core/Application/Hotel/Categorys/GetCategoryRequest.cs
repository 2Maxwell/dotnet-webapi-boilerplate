using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class GetCategoryRequest : IRequest<CategoryDto>
{
    public int Id { get; set; }
    public GetCategoryRequest(int id) => Id = id;
}

public class GetCategoryRequestHandler : IRequestHandler<GetCategoryRequest, CategoryDto>
{
    private readonly IRepository<Category> _repository;
    private readonly IStringLocalizer<GetCategoryRequestHandler> _localizer;

    public GetCategoryRequestHandler(IRepository<Category> repository, IStringLocalizer<GetCategoryRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<CategoryDto> Handle(GetCategoryRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Category, CategoryDto>)new CategoryByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["category.notfound"], request.Id));
}

public class CategoryByIdSpec : Specification<Category, CategoryDto>, ISingleResultSpecification
{
    public CategoryByIdSpec(int id) => Query.Where(x => x.Id == id);
}

