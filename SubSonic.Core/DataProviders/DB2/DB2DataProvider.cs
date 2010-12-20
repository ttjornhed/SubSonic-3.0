using System;
using SubSonic.DataProviders;
using SubSonic.Linq.Structure;
using SubSonic.Query;
using SubSonic.Schema;

namespace SubSonic.DataProviders.DB2
{
    /// <summary>
    /// This data provider is for IBM DB2, using the IBM Data Server Client.
    /// The provider string is typically "IBM.Data.DB2"
    /// </summary>
    public class DB2DataProvider : DbDataProvider
    {
        public DB2DataProvider(string connectionString, string providerName)
            : base(connectionString, providerName)
        {}

        public override ISchemaGenerator SchemaGenerator
        {
            get { return new DB2Schema(); }
        }

        public override IQueryLanguage QueryLanguage
        {
            get { return new DB2Language(this); }
        }

        public override string InsertionIdentityFetchString
        {
            get { return ""; }
        }

        public override string QualifyTableName(ITable tbl)
        {
            string qualifiedFormat = String.IsNullOrEmpty(tbl.SchemaName) ? "{1}" : "{0}.{1}";
            return String.Format(qualifiedFormat, tbl.SchemaName, tbl.Name);
        }

        public override string QualifyColumnName(IColumn column)
        {
            string qualifiedFormat = String.IsNullOrEmpty(column.SchemaName) ? "{1}.{2}" : "{0}.{1}.{2}";
            return String.Format(qualifiedFormat, column.Table.SchemaName, column.Table.Name, column.Name);
        }

        public override string QualifySPName(IStoredProcedure sp)
        {
            string qualifiedFormat = String.IsNullOrEmpty(sp.SchemaName) ? "{1}" : "{0}.{1}";
            return String.Format(qualifiedFormat, sp.SchemaName, sp.Name);
        }

        public override ISqlGenerator GetSqlGenerator(SqlQuery query)
        {
            return new DB2Generator(query);
        }
    }
}
