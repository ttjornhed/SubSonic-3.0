using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.DataProviders;
using SubSonic.Tests.Repositories.TestBases;

namespace SubSonic.Tests.Repositories
{

    public class SQLiteSimpleRepositoryTests : SimpleRepositoryTests
    {
        protected override string[] StringNumbers
        {
            get { return new string[] { "1", "2", "3" }; }
        }

        public SQLiteSimpleRepositoryTests() :
            base(ProviderFactory.GetProvider(new SQLitey().Connection, "System.Data.SQLite"))
        {
        }
    }
}
