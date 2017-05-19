using System.Data;
using Microsoft.Extensions.Logging;

namespace Sid.Dapper.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> CreateRepository<TEntity>(IDbConnection connection, ILogger logger) where TEntity : class;
    }
}
