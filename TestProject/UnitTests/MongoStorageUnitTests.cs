using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.UnitTests
{
    public class MongoStorageUnitTests
    {
        [Fact]
        public void TestInit()
        {
            var client = MongoDBStorage.MongoStorage.InitClient();
            var database = client.GetDatabase("local");
            Assert.NotNull(database);
        }
    }
}
