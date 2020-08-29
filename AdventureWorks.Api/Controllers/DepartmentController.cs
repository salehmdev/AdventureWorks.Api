using System;
using System.Threading.Tasks;
using AdventureWorks.Commands.Department;
using AdventureWorks.Query.Department;
using AdventureWorks.SqlData;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DepartmentGetById.QueryValidator _departmentGetByIdQueryValidator;

        public DepartmentController(IMediator mediator, DepartmentGetById.QueryValidator departmentGetByIdQueryValidator)
        {
            _mediator = mediator;
            _departmentGetByIdQueryValidator = departmentGetByIdQueryValidator;
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

            var validationResult = await _departmentGetByIdQueryValidator.ValidateAsync(query);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return ValidationProblem(ModelState);
            }

            return Ok(await _mediator.Send(query));
        }
    }
}
