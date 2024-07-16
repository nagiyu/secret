using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Models;
using Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Tests.Repositories
{
    [TestClass]
    public class SecretRepositoryTests
    {
        private SecretDbContext? _context;
        private SecretRepository? _repository;
        private IConfiguration? _configuration;

        [TestInitialize]
        public void TestInitialize()
        {
            var basePath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current Directory: {basePath}");

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();

            var connectionString = _configuration.GetConnectionString("TestDatabase");
            Console.WriteLine($"Connection String: {connectionString}");

            var options = new DbContextOptionsBuilder<SecretDbContext>()
                .UseNpgsql(connectionString)
                .Options;
            _context = new SecretDbContext(options);

            _repository = new SecretRepository(_context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context?.Dispose();
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllSecrets()
        {
            var repository = GetRepository();

            var beforeCount = (await repository.GetAllAsync()).Count();

            var secret1 = new Secret { Key = "TestKey", Value = "TestValue" };
            await repository.AddAsync(secret1);
            var secret2 = new Secret { Key = "TestKey", Value = "TestValue" };
            await repository.AddAsync(secret2);

            var afterCount = (await repository.GetAllAsync()).Count();

            Assert.AreEqual(beforeCount + 2, afterCount);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnSecret()
        {
            var repository = GetRepository();

            var secret = new Secret { Key = "TestKey", Value = "TestValue" };
            await repository.AddAsync(secret);

            var retrievedSecret = await repository.GetByIdAsync(secret.Id);
            Assert.IsNotNull(retrievedSecret);
            Assert.AreEqual("TestKey", retrievedSecret.Key);
            Assert.AreEqual("TestValue", retrievedSecret.Value);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateSecret()
        {
            var repository = GetRepository();

            var secret = new Secret { Key = "TestKey", Value = "TestValue" };
            await repository.AddAsync(secret);

            secret.Value = "UpdatedValue";
            await repository.UpdateAsync(secret);

            var updatedSecret = await repository.GetByIdAsync(secret.Id);
            Assert.IsNotNull(updatedSecret);
            Assert.AreEqual("UpdatedValue", updatedSecret.Value);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveSecret()
        {
            var repository = GetRepository();

            var secret = new Secret { Key = "TestKey", Value = "TestValue" };
            await repository.AddAsync(secret);

            await repository.DeleteAsync(secret.Id);

            var deletedSecret = await repository.GetByIdAsync(secret.Id);
            Assert.IsNull(deletedSecret);
        }

        private SecretRepository GetRepository()
        {
            if (_repository == null)
            {
                throw new Exception("Repository is null.");
            }

            return _repository;
        }
    }
}
