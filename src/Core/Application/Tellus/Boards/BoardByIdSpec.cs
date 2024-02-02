using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.Boards;
public class BoardByIdSpec : Specification<Board, BoardDto>, ISingleResultSpecification
{
    public BoardByIdSpec(int id) => Query.Where(c => c.Id == id);
}