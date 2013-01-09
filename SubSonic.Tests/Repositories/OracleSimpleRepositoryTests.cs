using SubSonic.DataProviders;

namespace SubSonic.Tests.Repositories
{
    public class OracleSimpleRepositoryTests : SimpleRepositoryTests
    {
        public OracleSimpleRepositoryTests() :
            base(ProviderFactory.GetProvider("NorthwindOracle"))
        {
            TestSupport.CleanTables(ProviderFactory.GetProvider("NorthwindOracle"), new [] {"NAIIWDS"});
        }
    }
}