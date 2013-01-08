using System;
using System.Data;
using SubSonic.DataProviders.Oracle;
using Xunit;
using System.Linq;

namespace SubSonic.Tests.Unit.DbTypeCoercion
{
    public class OracleGuidHandlingStrategyTests
    {
        private static readonly Guid Guid = new Guid("B40CF7B8-255C-4A63-8B42-75ABE6C2AB0B");
        
        [Fact]
        public void Should_Coerce_To_String_With_Each_Format()
        {
            var validFormats = new[] {"N", "D", "B", "P"};
            foreach (var format in validFormats)
                TestStringFormatCoercion(format);
        }

        private static void TestStringFormatCoercion(string format)
        {
            OracleGuidHandlingStrategy.DbType = DbType.String;
            OracleGuidHandlingStrategy.GuidStringFormat = format;
            var target = OracleGuidHandlingStrategy.ToString(Guid);
            Assert.Equal(Guid.ToString(format), target);
        }
    }
}