using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorks.Api.Data.SqlData;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Api.Commands.Department
{
    public class DepartmentCreate
    {
        public class Command : IRequest<DepartmentData>
        {
            public string Name { get; set; }
            public string GroupName { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, DepartmentData>
        {
            private readonly AdventureWorksContext _dbContext;

            public CommandHandler(AdventureWorksContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<DepartmentData> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _dbContext.Departments.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken: cancellationToken) != null)
                {
                    var validationFailure = new ValidationFailure(nameof(request.Name), $"'{request.Name}' already exists.", request.Name);
                    throw new ValidationException(new[] { validationFailure });
                }

                var department = (await _dbContext.Departments.AddAsync(new DepartmentData
                {
                    Name = request.Name,
                    GroupName = request.GroupName,
                    ModifiedDate = DateTime.UtcNow
                }, cancellationToken)).Entity;

                await _dbContext.SaveChangesAsync(cancellationToken);

                return department;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(AdventureWorksContext dbContext)
            {
                RuleFor(x => x.Name)
                    .NotEmpty();

                RuleFor(x => x.GroupName)
                    .NotEmpty();
            }
        }
    }
}