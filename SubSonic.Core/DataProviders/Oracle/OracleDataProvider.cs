using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.Linq.Structure;
using SubSonic.Query;
using SubSonic.Schema;
using SubSonic.SqlGeneration;
using SubSonic.SqlGeneration.Schema;
using SubSonic.DataProviders.Schema;

namespace SubSonic.DataProviders.Oracle {
	public class OracleDataProvider : DbDataProvider,IDataProvider {

        private string _InsertionIdentityFetchString = "";
        public override string InsertionIdentityFetchString { get { return _InsertionIdentityFetchString; } }

        public OracleDataProvider(string connectionString, string providerName)
            : base(connectionString, providerName)
        {}

		public override ISchemaGenerator SchemaGenerator {
			get { return new OracleSchema(); }
		}


		public override ISqlGenerator GetSqlGenerator(SqlQuery query) {
			return new OracleGenerator(query);
		}


		public override string QualifyTableName(ITable tbl) {
			string qualifiedFormat = String.IsNullOrEmpty(tbl.SchemaName) ? "{1}" : "{0}.{1}";
			return String.Format(qualifiedFormat, tbl.SchemaName, tbl.Name);
		}

		public override string QualifyColumnName(IColumn column) {
			string qualifiedFormat = String.IsNullOrEmpty(column.SchemaName) ? "{1}.{2}" : "{0}.{1}.{2}";
			return String.Format(qualifiedFormat, column.Table.SchemaName, column.Table.Name, column.Name);
		}

		public override string QualifySPName(IStoredProcedure sp) {
			string qualifiedFormat = String.IsNullOrEmpty(sp.SchemaName) ? "\"{1}\"" : "\"{0}\".\"{1}\"";
			return String.Format(qualifiedFormat, sp.SchemaName, sp.Name);
		}

        public override IQueryLanguage QueryLanguage { get { return new OracleLanguage(this); } }

        new public string ParameterPrefix
        {
            get { return ":"; }
        }
	}
}
