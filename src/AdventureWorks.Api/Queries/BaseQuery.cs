using System.Data;

namespace AdventureWorks.Api.Queries
{
    public abstract class BaseQuery
    {
        protected readonly IDbConnection DbConnection;
        protected readonly IDbTransaction Transaction;

        protected BaseQuery(IDbConnection dbConnection, IDbTransaction transaction = null)
        {
            DbConnection = dbConnection;
            Transaction = transaction;
        }
    }
}