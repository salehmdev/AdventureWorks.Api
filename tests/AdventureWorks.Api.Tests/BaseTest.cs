using System;
using AdventureWorks.Api.Commands;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AdventureWorks.Api.Tests
{
    public class BaseTest : IDisposable
    {
        protected AdventureWorksContext DbContext;
        protected Fixture Fixture;

        public BaseTest()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseInMemoryDatabase(Fixture.Build<string>().Create())
                .Options;

            DbContext = new AdventureWorksContext(options);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}