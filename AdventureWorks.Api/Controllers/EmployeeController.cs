﻿using System.Threading.Tasks;
using AdventureWorks.Query.Employee;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<EmployeeGetById.QueryResult> GetById(int id)
        {
            return await _mediator.Send(new EmployeeGetById.Query
            {
                Id = id
            });
        }
    }
}