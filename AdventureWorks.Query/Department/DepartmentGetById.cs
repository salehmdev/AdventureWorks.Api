using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;

namespace AdventureWorks.Query.Department
{
    public class DepartmentGetById
    {
        public class Query : IRequest<QueryResult>
        {
            public int Id { get; set; }
        }

        public class QueryResult
        {
            public int DepartmentId { get; set; }
            public string Name { get; set; }
            public string GroupName { get; set; }
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
                "SELECT * FROM [HumanResources].[Department] WHERE [DepartmentId] = @Id";
        }
    }
}