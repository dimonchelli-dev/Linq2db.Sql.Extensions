using IntegrationTests.Utility.Core;

namespace IntegrationTests.Utility.Sql;

public class SqlDbDockerCollectionFixture : DockerCollectionFixture<SqlDbCredentials>
{
    private static readonly DockerConfiguration Configuration = new(
        Image: "mcr.microsoft.com/mssql/server",
        ImageTag: "latest",
        ContainerNamePrefix: "IntegrationTestsSqlDb");

    public SqlDbDockerCollectionFixture()
        : base(
            BuildDockerRunner(),
            Configuration)
    {
    }

    private static SqlDbDockerRunner BuildDockerRunner() => new();
}