using System;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorks.SqlData;
using FluentValidation;
using MediatR;

namespace AdventureWorks.Commands.Department
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
            public CommandValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
                RuleFor(x => x.GroupName)
                    .NotEmpty();
            }
        }
    }
}