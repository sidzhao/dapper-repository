﻿using System.Linq;
using System.Threading.Tasks;
using Sid.Dapper.Repository.Tests.Entities;
using Sid.Dapper.Repository.Tests.Fixture;
using Xunit;
using System.IO;
using System.Reflection;

namespace Sid.Dapper.Repository.Tests.Tests
{
    public class RepositoryTest : IClassFixture<MsSqlDatabaseFixture>
    {
        private readonly MsSqlDatabaseFixture _fixture;

        public RepositoryTest(MsSqlDatabaseFixture msFixture)
        {
            _fixture = msFixture;
        }

        #region Insert

        [Fact]
        public async Task TestInsertAsync()
        {
            var insertRecord1 = new RecordsForInsertAsync { Name = "1" };
            var result = await _fixture.Db.SetEntity<RecordsForInsertAsync>().InsertAsync(insertRecord1);
            Assert.True(result);
            Assert.True(insertRecord1.Id == 1);

            var insertRecord2 = new RecordsForInsertAsync { Name = "2" };
            result = await _fixture.Db.SetEntity<RecordsForInsertAsync>().InsertAsync(insertRecord2);
            Assert.True(result);
            Assert.True(insertRecord2.Id == 2);
        }

        [Fact]
        public void TestInsert()
        {
            var insertRecord1 = new RecordsForInsert { Name = "1" };
            var result = _fixture.Db.SetEntity<RecordsForInsert>().Insert(insertRecord1);
            Assert.True(result);
            Assert.True(insertRecord1.Id == 1);

            var insertRecord2 = new RecordsForInsert { Name = "2" };
            result = _fixture.Db.SetEntity<RecordsForInsert>().Insert(insertRecord2);
            Assert.True(result);
            Assert.True(insertRecord2.Id == 2);
        }

        [Fact]
        public async Task TestInsertFile()
        {
            var fileEntity = new Entities.File { Content = System.Text.Encoding.UTF8.GetBytes("Test") };

            await _fixture.Db.SetEntity<Entities.File>().InsertAsync(fileEntity);
        }

        #endregion

        #region Update

        [Fact]
        public async Task TestUpdateAsync()
        {
            var record = await _fixture.Db.SetEntity<RecordsForUpdate>().FindAsync(p => p.Id == 1);
            Assert.True(record.Name == "1");

            record.Name = "10";
            var result = await _fixture.Db.SetEntity<RecordsForUpdate>().UpdateAsync(record);
            Assert.True(result);

            record = await _fixture.Db.SetEntity<RecordsForUpdate>().FindAsync(p => p.Id == 1);
            Assert.True(record.Name == "10");
        }

        [Fact]
        public void TestUpdate()
        {
            var record = _fixture.Db.SetEntity<RecordsForUpdate>().Find(p => p.Id == 2);
            Assert.True(record.Name == "2");

            record.Name = "20";
            var result = _fixture.Db.SetEntity<RecordsForUpdate>().Update(record);
            Assert.True(result);

            record = _fixture.Db.SetEntity<RecordsForUpdate>().Find(p => p.Id == 2);
            Assert.True(record.Name == "20");
        }

        #endregion

        #region Delete

        [Fact]
        public async Task TestDeleteAsync()
        {
            var result = await _fixture.Db.SetEntity<RecordsForDelete>().DeleteAsync(new RecordsForDelete { Id = 1 });
            Assert.True(result);

            var record = await _fixture.Db.SetEntity<RecordsForDelete>().FindAsync(p => p.Id == 1);
            Assert.True(record == null);
        }

        [Fact]
        public void TestDelete()
        {
            var result = _fixture.Db.SetEntity<RecordsForDelete>().Delete(new RecordsForDelete { Id = 2 });
            Assert.True(result);

            var record = _fixture.Db.SetEntity<RecordsForDelete>().Find(p => p.Id == 2);
            Assert.True(record == null);
        }

        #endregion

        #region Count

        [Fact]
        public void TestCount()
        {
            var count = _fixture.Db.SetEntity<User>().Count();
            Assert.Equal(3, count);

            count = _fixture.Db.SetEntity<Role>().Count(p => p.UserId == 1);
            Assert.Equal(3, count);

            count = _fixture.Db.SetEntity<Role>().Count(p => p.UserId == 99);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task TestCountAsync()
        {
            var count = await _fixture.Db.SetEntity<User>().CountAsync();
            Assert.Equal(3, count);

            count = await _fixture.Db.SetEntity<Role>().CountAsync(p => p.UserId == 1);
            Assert.Equal(3, count);

            count = await _fixture.Db.SetEntity<Role>().CountAsync(p => p.UserId == 99);
            Assert.Equal(0, count);
        }

        #endregion

        #region Any

        [Fact]
        public void TestAny()
        {
            var count = _fixture.Db.SetEntity<User>().Any();
            Assert.Equal(true, count);

            count = _fixture.Db.SetEntity<Role>().Any(p => p.UserId == 99);
            Assert.Equal(false, count);
        }

        [Fact]
        public async Task TestAnyAsync()
        {
            var count = await _fixture.Db.SetEntity<User>().AnyAsync();
            Assert.Equal(true, count);

            count = await _fixture.Db.SetEntity<Role>().AnyAsync(p => p.UserId == 99);
            Assert.Equal(false, count);
        }

        #endregion

        #region Select with lamada expression

        [Fact]
        public async Task TestSelectWithLamada()
        {
            var users = await _fixture.Db.SetEntity<User>().FindAllAsync(p=>p.Name == "Name1" && p.Id != 0 && p.Status != Status.Inactive);

            Assert.Equal(users.Count(), 1);
        }

        #endregion

        #region Mulitple Mapping Test

        [Fact]
        public void TestFindMultipleMapping()
        {
            var sql = @"SELECT * FROM Users
                        LEFT JOIN Cars ON Users.Id = Cars.UserId
                        LEFT JOIN Images On Users.Id = Images.UserId
                        WHERE Users.Deleted != 1";

            var users = _fixture.Db.SetEntity<User>().FindAll<Car, Image>(sql).ToList();
            Assert.Equal(3, users.Count);
            Assert.Equal(3, users[0].Cars.Count);
            Assert.Equal("Car1", users[0].Cars[0].CarName);
            Assert.Null(users[1].Cars);
            Assert.NotNull(users[0].Image);
            Assert.Equal("Image1", users[0].Image.Name);
            Assert.Null(users[1].Image);
            Assert.NotNull(users[2].Image);
            Assert.Equal("Image4", users[2].Image.Name);
        }

        [Fact]
        public void TestFindMultipleMapping1()
        {
            var sql = @"SELECT * FROM Users
                    LEFT JOIN Cars ON Users.Id = Cars.UserId
                    LEFT JOIN CarAddOns ON Cars.Id = CarAddOns.CarId
                    LEFT JOIN CarOptions ON Cars.Id = CarOptions.CarId
                    LEFT JOIN Images On Users.Id = Images.UserId
                    LEFT JOIN CarOptionImages ON CarOptions.Id = CarOptionImages.CarOptionId
                    LEFT JOIN Roles ON Users.Id = Roles.UserId
                    WHERE Users.Deleted != 1";

            var users = _fixture.Db.SetEntity<User>().FindAll<Car, CarAddOn, CarOption, Image, CarOptionImage, Role>(sql).ToList();
            Assert.Equal(3, users.Count);
            Assert.Equal(3, users[0].Cars.Count);
            Assert.Equal("Car1", users[0].Cars[0].CarName);
            Assert.Null(users[1].Cars);
            Assert.NotNull(users[0].Image);
            Assert.Equal("Image1", users[0].Image.Name);
            Assert.Null(users[1].Image);
            Assert.NotNull(users[2].Image);
            Assert.Equal("Image4", users[2].Image.Name);
            Assert.Equal(1, users[0].Cars[0].Options.Count);
            Assert.Equal(2, users[0].Cars[1].Options.Count);
            Assert.Equal("CarOption3", users[0].Cars[1].Options[1].OptionName);
            Assert.NotNull(users[0].Cars[0].Options[0].Image);
            Assert.Equal("CarOptionImage3", users[0].Cars[0].Options[0].Image.Name);
            Assert.Null(users[0].Cars[1].Options[1].Image);
        }

        [Fact]
        public void TestFindMultipleMappingForSpiltOn()
        {
            var sql = @"SELECT * FROM Dishes
                        LEFT JOIN DishOptions ON Dishes.DishId = DishOptions.DishId
                        LEFT JOIN DishImages ON Dishes.DishId = DishImages.DishId";
            var dishes = _fixture.Db.SetEntity<Dish>().FindAll<DishOption, DishImage>(sql).ToList();
            Assert.Equal(dishes.Count, 3);
            Assert.Equal(dishes[0].DishImages.Count, 1);
            Assert.Equal(dishes[0].DishOptions.Count, 2);
            Assert.Equal(dishes[0].DishOptions[0].Option, "DishOption1");
            Assert.Equal(dishes[1].DishImages.Count, 1);
            Assert.Null(dishes[1].DishOptions);
            Assert.Equal(dishes[2].DishImages.Count, 2);
            Assert.Equal(dishes[2].DishOptions.Count, 1);
        }

        [Fact]
        public async Task TestFindMultipleMappingAsync()
        {
            var sql = @"SELECT * FROM Users
                        LEFT JOIN Cars ON Users.Id = Cars.UserId
                        LEFT JOIN Images On Users.Id = Images.UserId
                        WHERE Users.Deleted != 1";

            var users = (await _fixture.Db.SetEntity<User>().FindAllAsync<Car, Image>(sql)).ToList();
            Assert.Equal(3, users.Count);
            Assert.Equal(3, users[0].Cars.Count);
            Assert.Equal("Car1", users[0].Cars[0].CarName);
            Assert.Null(users[1].Cars);
            Assert.NotNull(users[0].Image);
            Assert.Equal("Image1", users[0].Image.Name);
            Assert.Null(users[1].Image);
            Assert.NotNull(users[2].Image);
            Assert.Equal("Image4", users[2].Image.Name);
        }

        [Fact]
        public async Task TestFindMultipleMappingAsync1()
        {
            var sql = @"SELECT * FROM Users
                    LEFT JOIN Cars ON Users.Id = Cars.UserId
                    LEFT JOIN CarAddOns ON Cars.Id = CarAddOns.CarId
                    LEFT JOIN CarOptions ON Cars.Id = CarOptions.CarId
                    LEFT JOIN Images On Users.Id = Images.UserId
                    LEFT JOIN CarOptionImages ON CarOptions.Id = CarOptionImages.CarOptionId
                    LEFT JOIN Roles ON Users.Id = Roles.UserId
                    WHERE Users.Deleted != 1";

            var users = (await _fixture.Db.SetEntity<User>().FindAllAsync<Car, CarAddOn, CarOption, Image, CarOptionImage, Role>(sql)).ToList();
            Assert.Equal(3, users.Count);
            Assert.Equal(3, users[0].Cars.Count);
            Assert.Equal("Car1", users[0].Cars[0].CarName);
            Assert.Null(users[1].Cars);
            Assert.NotNull(users[0].Image);
            Assert.Equal("Image1", users[0].Image.Name);
            Assert.Null(users[1].Image);
            Assert.NotNull(users[2].Image);
            Assert.Equal("Image4", users[2].Image.Name);
            Assert.Equal(1, users[0].Cars[0].Options.Count);
            Assert.Equal(2, users[0].Cars[1].Options.Count);
            Assert.Equal("CarOption3", users[0].Cars[1].Options[1].OptionName);
            Assert.NotNull(users[0].Cars[0].Options[0].Image);
            Assert.Equal("CarOptionImage3", users[0].Cars[0].Options[0].Image.Name);
            Assert.Null(users[0].Cars[1].Options[1].Image);
        }

        [Fact]
        public async Task TestFindMultipleMappingAsyncForSpiltOn()
        {
            var sql = @"SELECT * FROM Dishes
                        LEFT JOIN DishOptions ON Dishes.DishId = DishOptions.DishId
                        LEFT JOIN DishImages ON Dishes.DishId = DishImages.DishId";
            var dishes = (await _fixture.Db.SetEntity<Dish>().FindAllAsync<DishOption, DishImage>(sql)).ToList();
            Assert.Equal(dishes.Count, 3);
            Assert.Equal(dishes[0].DishImages.Count, 1);
            Assert.Equal(dishes[0].DishOptions.Count, 2);
            Assert.Equal(dishes[0].DishOptions[0].Option, "DishOption1");
            Assert.Equal(dishes[1].DishImages.Count, 1);
            Assert.Null(dishes[1].DishOptions);
            Assert.Equal(dishes[2].DishImages.Count, 2);
            Assert.Equal(dishes[2].DishOptions.Count, 1);
        }

        #endregion
    }
}
