using Xunit;

namespace Linq2db.Sql.Extensions.Optimization.Tests.CollectionFixtures;

[CollectionDefinition(FixtureCollectionNames.SqlDbCollection)]
public class SqlDbFixtureCollection : ICollectionFixture<SqlDbFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}