﻿using System;
using System.Data;
using SubSonic.Linq.Structure;
using SubSonic.Query;
using SubSonic.Schema;
using System.Data.Common;

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

        protected override DbType CoerceDbType(DbType dataType)
        {
            switch (dataType)
            {
                case DbType.Guid:
                    return OracleGuidHandlingStrategy.DbType;
                default:
                    return base.CoerceDbType(dataType);
            }
        }

        protected override object CoerceValue(Query.QueryParameter param)
        {
            if (param.ParameterValue == null)
                return DBNull.Value;
            switch (param.DataType)
            {
                case DbType.Guid:
                    return OracleGuidHandlingStrategy.CoerceValue(param.ParameterValue);
                default:
                    return param.ParameterValue;
            }
        }

        #region Shared connection and transaction handling

        [ThreadStatic]
        private static DbConnection __sharedConnection;
        [ThreadStatic]
        private static DbTransaction __sharedTransaction;

        /// <summary>
        /// Gets or sets the current shared connection.
        /// </summary>
        /// <value>The current shared connection.</value>
        public override DbConnection CurrentSharedConnection
        {
            get { return __sharedConnection; }

            protected set
            {
                if (value == null)
                {
                    if (__sharedConnection != null)
                    {
                        __sharedConnection.Dispose();
                        __sharedConnection = null;
                    }
                }
                else
                {
                    __sharedConnection = value;
                    __sharedConnection.Disposed += __sharedConnection_Disposed;
                }
            }
        }

        public override DbTransaction CurrentSharedTransaction
        {
            get { return __sharedTransaction; }

            set
            {
                if (__sharedTransaction != null)
                {
                    try
                    {
                        __sharedTransaction.Dispose();
                    }
                    catch
                    {
                        // ignore errors.
                    }
                }
                __sharedTransaction = value;
            }
        }

        private static void __sharedConnection_Disposed(object sender, EventArgs e)
        {
            __sharedConnection = null;
        }

        #endregion
	}
}
