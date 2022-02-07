namespace IntegrationTests.Utility.Core;

public record ContainerInfo<TServiceCredentials>(string ContainerId, TServiceCredentials ServiceCredentials);