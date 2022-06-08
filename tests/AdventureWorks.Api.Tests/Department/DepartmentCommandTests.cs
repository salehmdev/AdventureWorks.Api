using System.Threading;
using System.Threading.Tasks;
using AdventureWorks.Api.Commands.Department;
using AutoFixture;
using NUnit.Framework;

namespace AdventureWorks.Api.Tests.Department
{
    [TestFixture]
    public class DepartmentCommandTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        public class Create : BaseTest
        {
            [Test]
            public async Task WhenCommandExecutes_ModelIsCreated()
            {
                var model = Fixture.Build<DepartmentCreate.Command>()
                    .Create();

                var commandHandler = new DepartmentCreate.CommandHandler(DbContext);

                var department = await commandHandler.Handle(model, new CancellationToken());

                Assert.IsNotNull(department);
                Assert.AreEqual(model.Name, department.Name);
                Assert.AreEqual(model.GroupName, department.GroupName);
            }
        }
    }
}