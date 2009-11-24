using System.Reflection;
using SubSonic.DataProviders;
using SubSonic.Extensions;
using SubSonic.Schema;
using Xunit;

namespace SubSonic.Tests.Migrations
{    
    public class OracleMigrationTests
    {
        IDataProvider _provider;
        Migrator migrator;

        public OracleMigrationTests()
        {
            _provider = ProviderFactory.GetProvider(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
            migrator=new Migrator(Assembly.GetExecutingAssembly());
        }

        [Fact]
        public void CreateTable_Should_CreateValid_SQL_For_SubSonicTest()
        {
            var shouldbe =
                @"CREATE TABLE ""SubSonicTests"" (
  ""Key"" VARCHAR2(16) NOT NULL,
  ""Thinger"" NUMBER(9,0) NOT NULL,
  ""Name"" VARCHAR2(255) NOT NULL,
  ""UserName"" VARCHAR2(500) NOT NULL,
  ""CreatedOn"" TIMESTAMP NOT NULL,
  ""Price"" NUMBER(10,2) NOT NULL,
  ""Discount"" NUMBER(10,2) NOT NULL,
  ""Lat"" NUMBER(10,3),
  ""Long"" NUMBER(10,3),
  ""SomeFlag"" VARCHAR2(1) NOT NULL,
  ""SomeNullableFlag"" VARCHAR2(1),
  ""LongText"" CLOB NOT NULL,
  ""MediumText"" VARCHAR2(800) NOT NULL,
  CONSTRAINT SubSonicTests_PK PRIMARY KEY (Key)
)";
            
            var sql = typeof (SubSonicTest).ToSchemaTable(_provider).CreateSql;
            Assert.Equal(shouldbe, sql);
        }

        [Fact]
        public void DropTable_Should_Create_Valid_Sql()
        {
            var shouldbe = @"DROP TABLE ""SubSonicTests"" PURGE";
            
            var sql = typeof(SubSonicTest).ToSchemaTable(_provider).DropSql.Trim();
            Assert.Equal(shouldbe, sql);
        }

        [Fact]
        public void DropColumnSql_Should_Create_Valid_Sql() {
            var shouldbe = @"ALTER TABLE ""SubSonicTests"" DROP COLUMN UserName";

            var sql = typeof (SubSonicTest).ToSchemaTable(_provider).DropColumnSql("UserName");
            Assert.Equal(shouldbe, sql);
        }

        [Fact]
        public void CreateColumnSql_Should_Create_Valid_Sql() {
            var shouldbe = @"ALTER TABLE ""SubSonicTests"" ADD UserName VARCHAR2(500) DEFAULT ' ' NOT NULL";

            var sql = typeof (SubSonicTest).ToSchemaTable(_provider).GetColumn("UserName").CreateSql;
            Assert.Equal(shouldbe, sql);
        }

        [Fact]
        public void AlterColumnSql_Should_Create_Valid_Sql() {
            var shouldbe = @"ALTER TABLE ""SubSonicTests"" MODIFY UserName VARCHAR2(500) NOT NULL";

            var sql = typeof(SubSonicTest).ToSchemaTable(_provider).GetColumn("UserName").AlterSql;
            Assert.Equal(shouldbe, sql);
        }

    }
}