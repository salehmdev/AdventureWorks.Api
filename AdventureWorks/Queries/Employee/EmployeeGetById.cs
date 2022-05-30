using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;

namespace AdventureWorks.Api.Queries.Employee
{
    public class EmployeeGetById
    {
        public class Query : IRequest<QueryResult>
        {
            public int Id { get; set; }
        }

        public class QueryResult
        {
            public int BusinessEntityId { get; set; }
            public string NationalIdNumber { get; set; }
            public string LoginId { get; set; }
            public string JobTitle { get; set; }
            public DateTime BirthDate { get; set; }
            public char Gender { get; set; }
            public DateTime ModifiedDate { get; set; }
        }

        public class QueryHandler : BaseQuery, IRequestHandler<Query, QueryResult>
        {
            public QueryHandler(IDbConnection dbConnection, IDbTransaction transaction = null)
                : base(dbConnection, transaction)
            {
            }

            public async Task<QueryResult> Handle(Query request, CancellationToken cancellationToken)
            {
                return await DbConnection.QuerySingleOrDefaultAsync<QueryResult>(CreateSql, request, Transaction);
            }

            private static string CreateSql =
                "SELECT * FROM [HumanResources].[Employee] WHERE [BusinessEntityID] = @Id";
        }

        // TODO: Add sample validation
    }
}