using System;
using System.Collections.Generic;
using System.Data;

namespace SubSonic.DataProviders.Oracle
{
    public class OracleGuidHandlingStrategy
    {
        private static readonly OracleGuidHandlingStrategy Instance = new OracleGuidHandlingStrategy();
        private DbType _dbType = DbType.String;
        private string _guidStringFormat = "N";
        private static readonly List<string> ValidGuidStringFormats = new List<string>(new []{"N", "D", "B", "P"}); 

        private OracleGuidHandlingStrategy()
        {
        }

        public static DbType DbType
        {
            get { return Instance._dbType; }
            set { Instance._dbType = value; }
        }

        public static String GuidStringFormat
        {
            get { return Instance._guidStringFormat; }
            set
            {
                if (!String.IsNullOrEmpty(value) && ValidGuidStringFormats.Contains(value.ToUpper()))
                    Instance._guidStringFormat = value.ToUpper();
            }
        }

        public static String ToString(Guid guid)
        {
            return guid.ToString(Instance._guidStringFormat);
        }

        public static object CoerceValue(object value)
        {
            var guid = (Guid) value;
            switch (Instance._dbType)
            {
                case DbType.Binary:
                    return guid.ToByteArray();
                default:
                    return guid.ToString(Instance._guidStringFormat);
            }
        }
    }
}