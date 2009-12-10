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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using SubSonic.Query;
using SubSonic.Schema;
using SubSonic.SqlGeneration.Schema;

namespace SubSonic.DataProviders
{
    public enum DataClient
    {
        SqlClient,
        //SqlCEClient,
        MySqlClient,
        //OleDbClient,
        OracleClient,
        OracleDataAccessClient,
        SQLite
    }

    public interface IDataProvider
    {
        //execution
        string DbDataProviderName { get; }
        string Name { get; }
        DataClient Client { get; set; }
        TextWriter Log { get; set; }

        /// <summary>
        /// Holds list of tables, views, stored procedures, etc.
        /// </summary>
        IDatabaseSchema Schema { get; set; }

        ISchemaGenerator SchemaGenerator { get; }
        string ParameterPrefix { get; }
        DbConnection CurrentSharedConnection { get; }
        string ConnectionString { get; }
        DbProviderFactory Factory { get; }
        ITable GetTableFromDB(string tableName);
        DbDataReader ExecuteReader(QueryCommand cmd);
        DataSet ExecuteDataSet(QueryCommand cmd);
        IList<T> ToList<T>(QueryCommand cmd) where T : new();
        object ExecuteScalar(QueryCommand cmd);
        T ExecuteSingle<T>(QueryCommand cmd) where T : new();
        int ExecuteQuery(QueryCommand cmd);
        ITable FindTable(string tableName);
        ITable FindOrCreateTable<T>() where T : new();
        ITable FindOrCreateTable(Type type);
        DbCommand CreateCommand();

        /// <summary>
        /// This method is a place to convert data types before they are added to a DbParameter's Value.
        /// This can be used to convert object to a different data type incase a DB provider doesn't natively
        /// support that type. For example, Oracle doesn't support 'bool' so it needs to be converted to a
        /// string or number instead.
        /// </summary>
        /// <param name="input">The original data.</param>
        /// <returns>The object to set the parameter's value to.</returns>
        object ConvertDataTypeForParameter(object input);

        //SQL formatting
        string QualifyTableName(ITable tbl);
        string QualifyColumnName(IColumn column);
        string QualifySPName(IStoredProcedure sp);
        //connection bits
        DbConnection InitializeSharedConnection(string connectionString);
        DbConnection InitializeSharedConnection();
        void ResetSharedConnection();
        DbConnection CreateConnection();
        void MigrateToDatabase<T>(Assembly assembly);
        void MigrateNamespaceToDatabase(string modelNamespace, Assembly assembly);
    }
}