using System;
using Linq2db.Sql.Extensions.Optimization.Tests.Database.Tables;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Linq2db.Sql.Extensions.Optimization.Tests.Database;

public class TestDataContext : DataConnection
{
    private const string ProviderName = "SqlServer.2017";
    private const int CommandTimeoutMinutes = 2;

    public TestDataContext(string connectionString)
        : base(ProviderName, connectionString, BuildMappingSchema())
        => CommandTimeout = (int)TimeSpan.FromMinutes(CommandTimeoutMinutes).TotalSeconds;

    internal virtual ITable<IntTable> IntTable => GetTable<IntTable>();
    internal virtual ITable<BigIntTable> BigIntTable => GetTable<BigIntTable>();
    internal virtual ITable<StringTable> StringTable => GetTable<StringTable>();
    internal virtual ITable<GuidTable> GuidTable => GetTable<GuidTable>();

    private static MappingSchema BuildMappingSchema()
    {
        var mappingScheme = MappingSchema.Default;
        mappingScheme.SetConvertExpression<DateTime, DateTime>(dt => DateTime.SpecifyKind(dt, DateTimeKind.Utc));
        return mappingScheme;
    }
}