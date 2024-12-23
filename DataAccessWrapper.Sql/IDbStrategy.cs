using Microsoft.Data.SqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccessWrapper.Sql
{
    internal interface IDbStrategy
    {
        public Task<bool> ExecuteAsync(Func<DbConnection, Task> func);
    }

    internal class HotColdStrategy : IDbStrategy
    {
        private readonly string _primaryDbAlias;
        private readonly string _secondaryDbAlias;
        public HotColdStrategy(string primaryDbAlias, string secondaryDbAlias)
        {
            _primaryDbAlias = primaryDbAlias;
            _secondaryDbAlias = secondaryDbAlias;
        }

        public async Task<bool> ExecuteAsync(Func<DbConnection, Task> func)
        {
            bool isPrimaryExecutionFailed = false;
            bool isSecondaryExecutionFailed = false;
            do
            {
                string dbAlias = !isPrimaryExecutionFailed ? _primaryDbAlias : _secondaryDbAlias;
                try
                {
                    using DbConnection connection = new SqlConnection(dbAlias);
                    await func(connection);
                }
                catch (Exception ex)
                {
                    if (!isPrimaryExecutionFailed)
                    {
                        isPrimaryExecutionFailed = true;
                    }
                    else if (!isSecondaryExecutionFailed)
                    {
                        isSecondaryExecutionFailed = true;
                    }
                    else
                    {
                        throw new AggregateException(ex.Message);
                    }
                }
            } while (!isPrimaryExecutionFailed && !isSecondaryExecutionFailed);

            return true;
        }
    }
}
