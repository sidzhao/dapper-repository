using System.Data;
using Moq;
using Sid.Dapper.Repository.Context;
using Sid.Dapper.Repository.Tests.Entities;
using Xunit;

namespace Sid.Dapper.Repository.Tests.Tests
{
    public class DbContextTest
    {
        [Fact]
        public void TestSetEntity()
        {
            var sqlConnection = new Mock<IDbConnection>();
            var db = new DbContext(sqlConnection.Object);
            
            var repositories = db.Repositories;

            var userRepository = db.SetEntity<User>();
            Assert.Equal(userRepository is IRepository<User>, true);
            Assert.Equal(repositories.Count == 1, true);
            Assert.Equal(repositories.ContainsKey(typeof(User)), true);

            for (int i = 0; i < 10; i++)
            {
                db.SetEntity<User>();
                Assert.Equal(repositories.Count == 1, true);
                Assert.Equal(repositories.ContainsKey(typeof(User)), true);
            }
        }
    }
}
