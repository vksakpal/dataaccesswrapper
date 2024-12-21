using System.Collections.Generic;
using System.Data;
using System.Data.Common;

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
        DataSet ExecuteAsync(string storedProcedureName, DbParameter[] parameters, List<string> outputTableNames);

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
        private readonly string _executionStrategy;

        private readonly string _primaryDbAlias;

        private readonly string _secondaryDbAlias;

        public DbRepository(string dbAlias)
        {
            _primaryDbAlias = dbAlias;
            _executionStrategy = "STANDALONE";
            _secondaryDbAlias = string.Empty;
        }


        public DbRepository(string primaryDbAlias, string secondaryDbAlias)
        {
            _executionStrategy = "HOTCOLD";
            _primaryDbAlias = primaryDbAlias;
            _secondaryDbAlias = secondaryDbAlias;
        }



        public DataSet ExecuteAsync(string storedProcedureName, DbParameter[] parameters, List<string> outputTableNames)
        {
            throw new System.NotImplementedException();
        }

        public DataSet ExecuteAsync(string storedProcedureName, List<string> outputTableNames)
        {
            throw new System.NotImplementedException();
        }

        DataSet IDbRepository.ExecuteAsync(string storedProcedureName, DbParameter[] parameters, List<string> outputTableNames)
        {
            throw new System.NotImplementedException();
        }

        DataSet IDbRepository.ExecuteAsync(string storedProcedureName, List<string> outputTableNames)
        {
            throw new System.NotImplementedException();
        }
    }
}
