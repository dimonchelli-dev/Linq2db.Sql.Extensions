using JetBrains.Annotations;

namespace IntegrationTests.Utility.Sql;

[PublicAPI]
public class SqlDbCredentials
{
    public IReadOnlyCollection<(string Key, string Value)> KeyValuePairs { get; }

    public SqlDbCredentials(IReadOnlyCollection<(string Key, string Value)> keyValuePairs)
        => KeyValuePairs = keyValuePairs;

    public string ToConnectionString()
    {
        return string.Join(separator: "; ", KeyValuePairs.Select(pair => $"{pair.Key}={pair.Value}"));
    }
}