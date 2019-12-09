using Dapper;
using Shared.Domains;
using Shared.Domains.Enumerations;
using System.Data;

namespace Shared.Contracts.DapperExtensions
{
    public interface IQueryParameters<T>
    {
        IQueryParameters<T> AddQueryParameter(string name, object value,
            QueryType queryType = QueryType.Equal,
            QueryCondition queryCondition = QueryCondition.And,
            DbType? dbType = null, int groupByOrder = 0);

        string GetSqlQuery(out DynamicParameters dynamicParameters);
    }
}