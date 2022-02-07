using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace IntegrationTests.Utility.Core;

internal abstract class DockerRunner<TServiceCredentials>
{
    internal async Task<ContainerInfo<TServiceCredentials>> EnsureDockerStartedAsync(
        DockerConfiguration configuration,
        CancellationToken cancellationToken)
    {
        await CleanupRunningContainers(
            configuration,
            cancellationToken);

        var progress = new Progress<JSONMessage>(
            message => Debug.WriteLine($"Create image - {message.Status}: {message.ProgressMessage}"));

        var dockerClient = GetDockerClient();
        var image = $"{configuration.Image}:{configuration.ImageTag}";
        await dockerClient.Images.CreateImageAsync(
            new ImagesCreateParameters
            {
                FromImage = image
            },
            authConfig: null,
            progress,
            cancellationToken);

        var parameters = BuildCreateContainerParameters(configuration);
        var serviceCredentials = ConfigureContainer(parameters);
        var container = await dockerClient
            .Containers
            .CreateContainerAsync(
                parameters,
                cancellationToken);

        await dockerClient
            .Containers
            .StartContainerAsync(
                container.ID,
                new ContainerStartParameters(),
                cancellationToken);

        await WaitServiceInDockerReadyAsync(
            serviceCredentials,
            cancellationToken);
        return new ContainerInfo<TServiceCredentials>(
            container.ID,
            serviceCredentials);
    }

    internal static async Task EnsureDockerStoppedAndRemovedAsync(
        string? dockerContainerId,
        CancellationToken cancellationToken)
    {
        if (dockerContainerId == null)
        {
            return;
        }

        var dockerClient = GetDockerClient();
        await dockerClient.Containers.StopContainerAsync(
            dockerContainerId,
            new ContainerStopParameters(),
            cancellationToken);
        await dockerClient.Containers.RemoveContainerAsync(
            dockerContainerId,
            new ContainerRemoveParameters(),
            cancellationToken);
    }

    private async Task WaitServiceInDockerReadyAsync(
        TServiceCredentials credentials,
        CancellationToken cancellationToken)
    {
        const int maxWaitingPeriodInSeconds = 60;
        var maxWaitingPeriod = TimeSpan.FromSeconds(maxWaitingPeriodInSeconds);
        var softCancellationToken = new CancellationTokenSource(maxWaitingPeriod);

        const int iterationDelayInSeconds = 1;
        var iterationDelay = TimeSpan.FromSeconds(iterationDelayInSeconds);

        var connectionEstablished = false;

        while (!connectionEstablished
               && !softCancellationToken.IsCancellationRequested)
        {
            connectionEstablished = await IsServiceInDockerReadyAsync(
                credentials,
                cancellationToken);
            if (!connectionEstablished)
            {
                Debug.WriteLine(message: "Wait service in docker ready");
            }

            await Task.Delay(
                iterationDelay,
                cancellationToken);
        }

        if (connectionEstablished)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Connection to the SQL docker database could not be established within {maxWaitingPeriod}");
    }

    protected abstract Task<bool> IsServiceInDockerReadyAsync(
        TServiceCredentials credentials,
        CancellationToken cancellationToken);

    private static CreateContainerParameters BuildCreateContainerParameters(
        DockerConfiguration configuration) =>
        new()
        {
            Name = configuration.ContainerNamePrefix + "_" + Guid.NewGuid(),
            Image = $"{configuration.Image}:{configuration.ImageTag}"
        };

    protected abstract TServiceCredentials ConfigureContainer(
        CreateContainerParameters parameters);

    private static async Task CleanupRunningContainers(
        DockerConfiguration configuration,
        CancellationToken cancellationToken)
    {
        var dockerClient = GetDockerClient();

        var runningContainers = await dockerClient.Containers
            .ListContainersAsync(new ContainersListParameters(), cancellationToken);

        var createdBefore = DateTime.UtcNow.AddHours(value: -1);
        var candidates = runningContainers
            .Where(cont => cont.Names.Any(n => n.Contains(configuration.ContainerNamePrefix)) && cont.Created <= createdBefore);
        foreach (var runningContainer in candidates)
        {
            try
            {
                await EnsureDockerStoppedAndRemovedAsync(
                    runningContainer.ID,
                    cancellationToken);
            }
            catch
            {
                // Ignoring failures to stop running containers
            }
        }
    }

    private static DockerClient GetDockerClient()
    {
        const string windowsDockerUri = "npipe://./pipe/docker_engine";
        const string unixDockerUri = "unix:///var/run/docker.sock";

        var dockerUri = IsRunningOnWindows()
            ? windowsDockerUri
            : unixDockerUri;

        return new DockerClientConfiguration(new Uri(dockerUri))
            .CreateClient();
    }

    protected static string GetFreePort(string preferredPort)
    {
        return GetFreePorts(count: 1, new []{ preferredPort })[index: 0];
    }

    private static IReadOnlyList<string> GetFreePorts(
        int count,
        IReadOnlyCollection<string>? preferredPorts = null)
    {
        // https://www.iana.org/assignments/service-names-port-numbers/service-names-port-numbers.txt
        const int minPortNumberForDynamicRange = 49152;

        var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        var usedPorts = Enumerable.Empty<int>()
            .Concat(ipProperties.GetActiveTcpConnections().Select(c => c.LocalEndPoint.Port))
            .Concat(ipProperties.GetActiveTcpListeners().Select(l => l.Port))
            .Concat(ipProperties.GetActiveUdpListeners().Select(l => l.Port))
            .Select(p => p.ToString())
            .ToHashSet();

        if (preferredPorts == null)
        {
            return FilterFreePorts(GetPortsRange(), count);
        }

        var preferred = FilterFreePorts(preferredPorts, count);
        if (preferred.Count == count)
        {
            return preferred;
        }

        var otherCandidates = GetPortsRange()
           .Except(preferred);
        var other = FilterFreePorts(otherCandidates, count - preferred.Count);
        return preferred
           .Union(other)
           .ToArray();

        IEnumerable<string> GetPortsRange()
            => Enumerable.Range(minPortNumberForDynamicRange, IPEndPoint.MaxPort)
               .Select(p => p.ToString());

        IReadOnlyList<string> FilterFreePorts(IEnumerable<string> candidates, int maxCount)
        {
            return candidates
               .Except(usedPorts)
               .Take(maxCount)
               .Select(p => p.ToString())
               .ToArray();
        }
    }

    private static bool IsRunningOnWindows()
        => Environment.OSVersion.Platform == PlatformID.Win32NT;
}