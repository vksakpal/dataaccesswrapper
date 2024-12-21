namespace DataAccessWrapper.Sql
{

    /// <summary>
    /// Data Access Factory Interface
    /// </summary>
    public interface IDataAccessFactory
    {
        /// <summary>
        /// Returns DbRepository Interface for single database
        /// </summary>
        /// <param name="dbAlias"></param>
        /// <returns></returns>
        IDbRepository GetRepository(string dbAlias);

        /// <summary>
        /// Returns DbRepository Interface for primary and secondary database
        /// </summary>
        /// <param name="primaryDbAlias"></param>
        /// <param name="secondaryDbAlias"></param>
        /// <remarks>If primary db execution fails, it will try to execute the secondary database</remarks>
        /// <returns></returns>

        IDbRepository GetDbRepository(string primaryDbAlias, string secondaryDbAlias);
    }

    public class DataAccessRepository : IDataAccessFactory
    {        
        public IDbRepository GetDbRepository(string primaryDbAlias, string secondaryDbAlias)
        {
            return new DbRepository(primaryDbAlias, secondaryDbAlias);
        }

        public IDbRepository GetRepository(string dbAlias)
        {
            return new DbRepository(dbAlias);
        }
    }

}
