// 
//   SubSonic - http://subsonicproject.com
// 
//   The contents of this file are subject to the New BSD
//   License (the "License"); you may not use this file
//   except in compliance with the License. You may obtain a copy of
//   the License at http://www.opensource.org/licenses/bsd-license.php
//  
//   Software distributed under the License is distributed on an 
//   "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or
//   implied. See the License for the specific language governing
//   rights and limitations under the License.
// 


using SubSonic.Tests.Unit.Linq.TestBases;
using SubSonic.DataProviders;
using SubSonic.Tests.Unit.Linq.SqlStrings;
using System.Linq;
using SubSonic.Extensions;

namespace SubSonic.Tests.Unit.Linq
{
	public class OracleTests : SelectTests 
	{
		public OracleTests()
		{
			_selectTestsSql = new OracleSelectTestsSql();
			_db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
        }
    }

    public class OracleStringTests : StringTests
    {
        public OracleStringTests()
        {
            _stringTestsSql = new OracleStringTestsSql();
            _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
        }
    }

    public class OracleNumberTests : NumberTests
    {
        public OracleNumberTests()
        {
            _numberTestsSql = new OracleNumberTestsSql();
            _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
        }
    }

    public class OracleDateTests : DateTests
    {
        public OracleDateTests()
        {
            _dateTestsSql = new OracleDateTestsSql();
            _db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
        }
    }

    public class OracleSpecificTests : LinqTestsBase
    {
        private OracleSelectTestsSql _selectTestsSql;

        public OracleSpecificTests()
		{
			_selectTestsSql = new OracleSelectTestsSql();
			_db = new TestDB(TestConfiguration.OracleTestConnectionString, DbClientTypeName.OracleDataAccess);
            _db.Provider.ExecuteDetachedForDebug = true;
        }

        [Xunit.Fact]
        public void Ora_Where_Any_Generates_Valid_Query()
        {
            var result = _db.Customers.Where(c => c.CompanyName == "foo").Any();
            var debugInfo = _db.Provider.LastExecuteDebug;

            AssertEqualIgnoringExtraWhitespaceAndCarriageReturn(_selectTestsSql.Ora_Where_Any_Generates_Valid_Query, debugInfo.SQLStatement);
        }

        [Xunit.Fact]
        public void Ora_Any_Generates_Valid_Query()
        {
            var result = _db.Customers.Any(c => c.CompanyName == "foo");
            var debugInfo = _db.Provider.LastExecuteDebug;

            AssertEqualIgnoringExtraWhitespaceAndCarriageReturn(_selectTestsSql.Ora_Any_Generates_Valid_Query, debugInfo.SQLStatement);
        }

        [Xunit.Fact]
        public void Ora_Predecate_With_EqualsTrue_Generates_Valid_Query()
        {
            var result = _db.Customers.Where(c => c.ContactName.StartsWith("C") == true);
            var debugInfo = result.GetQueryText();

            AssertEqualIgnoringExtraWhitespaceAndCarriageReturn(_selectTestsSql.Ora_Predecate_With_EqualsTrue_Generates_Valid_Query, debugInfo);
        }

        [Xunit.Fact]
        public void Ora_Predecate_Without_EqualsTrue_Generates_Valid_Query()
        {
            var result = _db.Customers.Where(c => c.ContactName.StartsWith("C"));
            var debugInfo = result.GetQueryText();

            AssertEqualIgnoringExtraWhitespaceAndCarriageReturn(_selectTestsSql.Ora_Predecate_Without_EqualsTrue_Generates_Valid_Query, debugInfo);
        }
    }
}
