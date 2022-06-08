using System.Threading.Tasks;
using AdventureWorks.Api.Commands.Department;
using FluentValidation;
using NUnit.Framework;

namespace AdventureWorks.Api.Tests.Department
{
    [TestFixture]
    public class DepartmentCommandValidatorTests
    {
        public class Create : BaseTest
        {
            [Test]
            public async Task WhenCommandIsNotValid_ErrorIsThrown()
            {
                var command = new DepartmentCreate.Command
                {
                    Name = "Test"
                };

                var validator = new DepartmentCreate.CommandValidator();

                Assert.ThrowsAsync<ValidationException>(() => validator.ValidateAndThrowAsync(command));
            }

            [Test]
            public async Task WhenCommandIsValid_NoErrorIsThown()
            {
                var command = new DepartmentCreate.Command
                {
                    Name = "Test",
                    GroupName = "Test Group"
                };

                var validator = new DepartmentCreate.CommandValidator();

                Assert.DoesNotThrowAsync(() => validator.ValidateAndThrowAsync(command));
            }
        }
    }
}