using System.Threading.Tasks;
using AdventureWorks.Commands.Department;
using AdventureWorks.Query.Department;
using AdventureWorks.SqlData;
using FluentValidation;
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

        [HttpPost]
        public async Task<DepartmentData> Create([FromBody] DepartmentCreate.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new DepartmentGetById.Query
            {
                Id = id
            };

            try
            {
                return Ok(await _mediator.Send(query));
            }
            catch (ValidationException e)
            {
                foreach (var validationFailure in e.Errors)
                {
                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }

                return ValidationProblem(ModelState);
            }
        }
    }
}
