using System.Threading.Tasks;
using AdventureWorks.Commands.Department;
using AutoFixture;
using FluentValidation;
using NUnit.Framework;

namespace AdventureWorks.Commands.Tests.Department
{
    public class DepartmentCommandValidatorTests
    {
        [TestFixture]
        public class CreateCommandValidatorTests : BaseTest
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