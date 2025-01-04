using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccessWrapper.Sql
{
    public interface IDbRepository
    {
        /// <summary>
        /// Executes the stored procedure and returns the dataset
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="outputTableNames"></param>
        /// <returns></returns>
        Task<DataSet> ExecuteAsync(string storedProcedureName, DbParameter[] parameters, List<string> outputTableNames);

        /// <summary>
        /// Executes the stored procedure and returns the dataset
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="outputTableNames"></param>
        /// <returns></returns>
        DataSet ExecuteAsync(string storedProcedureName, List<string> outputTableNames);
    }

    public class DbRepository : IDbRepository
    {


        private readonly string _primaryDbAlias;
        private readonly ExecutionStrategy _executionStrategy;
        private readonly string _secondaryDbAlias;

        public DbRepository(string dbAlias)
        {
            _primaryDbAlias = dbAlias;
            _executionStrategy = ExecutionStrategy.StandAlone;
            _secondaryDbAlias = string.Empty;
        }


        public DbRepository(string primaryDbAlias, string secondaryDbAlias)
        {
            _executionStrategy = ExecutionStrategy.HotCold;
            _primaryDbAlias = primaryDbAlias;
            _secondaryDbAlias = secondaryDbAlias;
        }


        async Task<DataSet> IDbRepository.ExecuteAsync(string storedProcedureName, DbParameter[] parameters, List<string> outputTableNames)
        {
            DataSet? dataSet = new DataSet();
            if (_executionStrategy == ExecutionStrategy.HotCold)
            {

                IDbStrategy dbStrategy = new HotColdStrategy(_primaryDbAlias, _secondaryDbAlias);
                _ = await dbStrategy.ExecuteAsync(async (connection) =>
                {
                    await connection.OpenAsync();

                    using DbCommand command = connection.CreateCommand();
                    command.CommandText = storedProcedureName;
                    using DbDataReader dbDataReader = command.ExecuteReader();
                    dataSet = new DataSet();
                    if (dbDataReader != null)
                    {
                        while (dbDataReader.Read())
                        {
                            dataSet.Tables.Add().Load(dbDataReader);
                        }
                    }

                });
            }

            return dataSet;
        }

        DataSet IDbRepository.ExecuteAsync(string storedProcedureName, List<string> outputTableNames)
        {
            throw new System.NotImplementedException();
        }
    }
}
