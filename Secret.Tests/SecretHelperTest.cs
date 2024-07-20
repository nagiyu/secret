using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Diagnostics;

namespace Secret.Tests
{
    [TestClass]
    public class SecretHelperTest
    {
        private IConfiguration configuration;

        public SecretHelperTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
        }

        [TestMethod]
        public void GetSecretTest()
        {
            var key = configuration["SecretKey"];
            Assert.IsNotNull(key);
            var secret = SecretHelper.GetSecret(key).Result;
            Assert.IsNotNull(secret);
            Debug.WriteLine(secret);
        }
    }
}