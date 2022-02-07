namespace IntegrationTests.Utility.Core;

public abstract class DockerCollectionFixture<TServiceCredentials>
{
    private readonly DockerRunner<TServiceCredentials> _dockerRunner;
    private readonly DockerConfiguration _configuration;

    private string? _containerId;

    internal DockerCollectionFixture(
        DockerRunner<TServiceCredentials> dockerRunner,
        DockerConfiguration configuration)
    {
        _dockerRunner = dockerRunner;
        _configuration = configuration;
    }

    public async Task<TServiceCredentials> InitializeAsync(
        CancellationToken cancellationToken)
    {
        (string containerId, TServiceCredentials serviceCredentials) = await _dockerRunner.EnsureDockerStartedAsync(
            _configuration,
            cancellationToken);
        _containerId = containerId;
        return serviceCredentials;
    }

    public Task TerminateAsync(
        CancellationToken cancellationToken)
        => DockerRunner<TServiceCredentials>.EnsureDockerStoppedAndRemovedAsync(
            _containerId,
            cancellationToken);
}