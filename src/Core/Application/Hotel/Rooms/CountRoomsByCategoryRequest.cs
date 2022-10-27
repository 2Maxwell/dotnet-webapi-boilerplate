using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Rooms;

public class CountRoomsByCategoryRequest : BaseFilter, IRequest<int>
{
    public int CategoryId { get; set; }
    public CountRoomsByCategoryRequest(int categoryId) => CategoryId = categoryId;
}

public class CountRoomsByCategoryRequestSpec : Specification<Room, int>, ISingleResultSpecification
{
    public CountRoomsByCategoryRequestSpec(CountRoomsByCategoryRequest request)
    {
        Query.Where(c => c.CategoryId == request.CategoryId);
    }
}

public class CountRoomsByCategoryRequestHandler : IRequestHandler<CountRoomsByCategoryRequest, int>
{
    private readonly IReadRepository<Room> _repository;
    public CountRoomsByCategoryRequestHandler(IReadRepository<Room> repository) => _repository = repository;
    public async Task<int> Handle(CountRoomsByCategoryRequest request, CancellationToken cancellationToken)
    {
        var spec = new CountRoomsByCategoryRequestSpec(request);
        int count = await _repository.CountAsync(spec, cancellationToken);
        return count;
    }
}