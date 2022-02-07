using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IntegrationTests.Utility.Sql;
using JetBrains.Annotations;
using Linq2db.Sql.Extensions.Optimization.Tests.Database;
using LinqToDB.Data;
using Xunit;

namespace Linq2db.Sql.Extensions.Optimization.Tests.CollectionFixtures;

[PublicAPI]
public class SqlDbFixture : IAsyncLifetime
{
    private readonly SqlDbDockerCollectionFixture _fixture;

    public SqlDbFixture()
        => _fixture = new SqlDbDockerCollectionFixture();

    public SqlDbCredentials? DbCredentials { get; private set; }

    public async Task InitializeAsync()
    {
        var serverCredentials = await _fixture.InitializeAsync(CancellationToken.None);
        var serverConnectionString = serverCredentials.ToConnectionString();

        await CreateDatabase(serverConnectionString, CancellationToken.None);

        var pairs = serverCredentials.KeyValuePairs.ToList();
        pairs.Add(("Database", "Linq2dbTestsDatabase"));
        var databaseCredentials = new SqlDbCredentials(pairs);
        var databaseConnectionString = databaseCredentials.ToConnectionString();

        await InitializeDatabase(databaseConnectionString, CancellationToken.None);

        DbCredentials = new SqlDbCredentials(pairs);
    }

    public async Task DisposeAsync()
    {
        await _fixture.TerminateAsync(CancellationToken.None);
    }

    private static async Task CreateDatabase(
        string connectionString,
        CancellationToken cancellationToken)
    {
        var context = new TestDataContext(connectionString);
        var createDatabaseSql = await GetSql(scriptFileName: "CreateDatabaseScript.sql");
        await context.ExecuteAsync(createDatabaseSql, cancellationToken);
    }

    private static async Task InitializeDatabase(
        string connectionString,
        CancellationToken cancellationToken)
    {
        await using var context = new TestDataContext(connectionString);
        var initialSql = await GetSql(scriptFileName: "InitialScript.sql");
        await context.ExecuteAsync(initialSql, cancellationToken);
    }

    private static async  Task<string> GetSql
        (string scriptFileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var initialSqlFileName = $"Linq2db.Sql.Extensions.Optimization.Tests.Database.Scripts.{scriptFileName}";
        await using var stream = assembly.GetManifestResourceStream(initialSqlFileName);
        using var reader = new StreamReader(stream ?? throw new InvalidOperationException(message: "Initial sql script not found"));
        return await reader.ReadToEndAsync();
    }
}