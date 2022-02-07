using System;
using Linq2db.Sql.Extensions.Optimization.Tests.CollectionFixtures;

namespace Linq2db.Sql.Extensions.Optimization.Tests.Database;

public static class SqlDbFixtureExtensions
{
    public static TestDataContext BuildDataContext(this SqlDbFixture fixture) =>
        new(fixture.DbCredentials?.ToConnectionString()
         ?? throw new InvalidOperationException(message: "database not defined"));
}