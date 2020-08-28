using System.Threading.Tasks;
using AdventureWorks.Query.Department;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<DepartmentGetById.QueryResult> GetById(int id)
        {
            return await _mediator.Send(new DepartmentGetById.Query
            {
                Id = id
            });
        }
    }
}
