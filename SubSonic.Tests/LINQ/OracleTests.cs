using SubSonic.DataProviders;
using SubSonic.Tests.Linq;
using SubSonic.Tests.Linq.TestBases;
using Xunit;
using System.Linq;
using SubSonic.Linq.Structure;

namespace SubSonic.Tests.LINQ
{
    /**
     *  Tests for Oracle using ODP.NET.
     *  
     *  Since not everyone has a full blown Oracle DB laying around for testing, you can use Oracle XE
     *   which is available from Oracle here: http://www.oracle.com/technology/software/products/database/xe/index.html
     *  At this time, XE is based on 10g and full-blown Oracle is 11g, but I think XE should be good enough
     *   for testing. The query language hasn't changed significantly. However, be forewarned; Oracle
     *   has managed to bloat the "Express Edition" to requiring at least 1.5 GB free disk space! Thanks guys!
     *   
     *  Also note that this is dependant on the Oracle Data Provider for .NET (ODP.NET) Oracle.DataAccess
     *   assembly which can also be obtained directly from Oracle.
     *   
     *  1) Install Oracle XE
     *  2) Set SYSTEM account pwd to 'system' (or change the connection string in <see cref="TestConfiguration"/>)
     *  3) Open Oracle XE web interface (http://127.0.0.1:8080/apex)
     *  4) Click the dropdown arrow on the "SQL" button, and choose: SQL Scripts | Upload
     *  5) Browse to and Upload: SubSonic\DbScripts\Northwind_Oracle.sql and Northwind_Oracle_Data.sql
     *  6) Click on the uploaded Northwind_Oracle script to open script editor. Click "Run" button. Check results for errors.
     *  7) Click on the uploaded Northwind_Oracle_Data script to open script editor. Click "Run" button. Check results for errors.
     */

    // [TestFixture]
    public class OracleSelectTests : SubSonic.Tests.Linq.SelectTests
    {
        public OracleSelectTests()
        {
            _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
            var setup = new Setup(_db.Provider);
            setup.DropTestTables();
            setup.CreateTestTable();
            setup.LoadTestData();
        }
    }

    // [TestFixture]
    public class OracleNumberTests : NumberTests
    {
        public OracleNumberTests()
        {
            _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
            var setup = new Setup(_db.Provider);
            setup.DropTestTables();
            setup.CreateTestTable();
            setup.LoadTestData();
        }
    }

    // [TestFixture]
    public class OracleStringTests : StringTests
    {
        public OracleStringTests()
        {
            _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
            var setup = new Setup(_db.Provider);
            setup.DropTestTables();
            setup.CreateTestTable();
            setup.LoadTestData();
        }
    }

    // [TestFixture]
    public class OracleDateTests : DateTests
    {
        public OracleDateTests()
        {
            _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
            var setup = new Setup(_db.Provider);
            setup.DropTestTables();
            setup.CreateTestTable();
            setup.LoadTestData();
        }
    }

    /// <summary>
    /// Tests the parameterization of queries.
    /// Unlike the other data providers, we should turn all values into bind variables, instead of just strings.
    /// This is due to the way Oracle does SQL parsing and execution plan caching, and the way it impacts scalability.
    /// </summary>
    public class OracleQueryParameterizationTests
    {
        [Fact]
        public void CheckParameterization()
        {
            var _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
            var expr = _db.Categories.Where(x => x.CategoryID == 123 || x.CategoryName == "abc").Select(x => x.CategoryID).Expression;
            var plan = _db.QueryProvider.GetQueryPlan(expr);

            // the literals should be in the plan as named values
            Assert.Contains("CategoryName = :", plan);
            Assert.Contains("CategoryID = :", plan);
            Assert.Contains("(Object)\"abc\"", plan);
            Assert.Contains("(Object)123", plan);
        }

        [Fact]
        public void TakeNumberIsNotParameterizedWhenUsingTake()
        {
            var _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
            var expr = _db.Categories.Where(x => x.CategoryID == 999).Select(x => x.CategoryID).Skip(100).Take(1).Expression;
            var plan = _db.QueryProvider.GetQueryPlan(expr);

            // 999 should habe been turned into a parameter
            Assert.Contains("CategoryID = :", plan);
            Assert.Contains("(Object)999", plan);

            // the 1 for Take() should have been left as a literal
            Assert.Contains("rn >= 100 AND rn <= 101", plan);
        }
    }
}
