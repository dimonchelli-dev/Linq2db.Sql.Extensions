using Docker.DotNet.Models;
using IntegrationTests.Utility.Core;
using Microsoft.Data.SqlClient;

namespace IntegrationTests.Utility.Sql;

internal class SqlDbDockerRunner : DockerRunner<SqlDbCredentials>
{
    protected override SqlDbCredentials ConfigureContainer(CreateContainerParameters parameters)
    {
        const string username = "sa";
        const string password = "Passw0rd";

        const string preferredPort = "1433";
        var port = GetFreePort(preferredPort);

        parameters.Env = new List<string>
        {
            "ACCEPT_EULA=Y",
            $"SA_PASSWORD={password}"
        };

        parameters.HostConfig = new HostConfig
        {
            PortBindings = new Dictionary<string, IList<PortBinding>>
            {
                {
                    "1433/tcp",
                    new[]
                    {
                        new PortBinding
                        {
                            HostPort = port
                        }
                    }
                }
            }
        };

        return new SqlDbCredentials(new []
        {
            ("Server", $"localhost,{port}"),
            ("User ID", username),
            ("Password", password),
            ("TrustServerCertificate", "True")
        });
    }

    protected override async Task<bool> IsServiceInDockerReadyAsync(
        SqlDbCredentials credentials,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var sqlConnection = new SqlConnection(credentials.ToConnectionString());
            await sqlConnection.OpenAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}