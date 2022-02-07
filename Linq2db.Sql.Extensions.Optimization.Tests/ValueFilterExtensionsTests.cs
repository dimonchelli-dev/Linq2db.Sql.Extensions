using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Linq2db.Sql.Extensions.Optimization.Extensions;
using Linq2db.Sql.Extensions.Optimization.Tests.CollectionFixtures;
using Linq2db.Sql.Extensions.Optimization.Tests.Database;
using Linq2db.Sql.Extensions.Optimization.Tests.Database.Tables;
using LinqToDB;
using Xunit;

namespace Linq2db.Sql.Extensions.Optimization.Tests;

[Trait(name: "Category",value: "Integration")]
[Collection(FixtureCollectionNames.SqlDbCollection)]
public class ValueFilterExtensionsTests
{
    private readonly SqlDbFixture _fixture;

    public ValueFilterExtensionsTests(SqlDbFixture fixture)
        => _fixture = fixture;

    #region int

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task FilterByIntValues(int filteredValuesCount)
    {
        await using var context = _fixture.BuildDataContext();
        var values = await GetValues<int, IntTable>(
            filteredValuesCount,
            context.IntTable);
        var foundItems = await context.FilterByValues(
            context.IntTable,
            table => table.Value,
            values,
            CancellationToken.None);
        var foundValues = foundItems
            .Select(table => table.Value)
            .ToArray();
        Assert.True(values.OrderBy(_ => _)
            .SequenceEqual(foundValues.OrderBy(_ => _)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task FilterByIntValuesWithResult(int filteredValuesCount)
    {
        await using var context = _fixture.BuildDataContext();
        var values = await GetValues<int, IntTable>(
            filteredValuesCount,
            context.IntTable);
        var foundValues = await context.FilterByValues(
            context.IntTable,
            table => table.Value,
            values,
            async table => await table.Select(t => t.Value).ToArrayAsync(),
            CancellationToken.None);
        Assert.True(values.OrderBy(_ => _)
            .SequenceEqual(foundValues.OrderBy(_ => _)));
    }

    #endregion

    #region long

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task FilterByLongValues(int filteredValuesCount)
    {
        await using var context = _fixture.BuildDataContext();
        var values = await GetValues<long, BigIntTable>(
            filteredValuesCount,
            context.BigIntTable);
        var foundItems = await context.FilterByValues(
            context.BigIntTable,
            table => table.Value,
            values,
            CancellationToken.None);
        var foundValues = foundItems
            .Select(table => table.Value)
            .ToArray();
        Assert.True(values.OrderBy(_ => _)
            .SequenceEqual(foundValues.OrderBy(_ => _)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task FilterByLongValuesWithResult(int filteredValuesCount)
    {
        await using var context = _fixture.BuildDataContext();
        var values = await GetValues<long, BigIntTable>(
            filteredValuesCount,
            context.BigIntTable);
        var foundValues = await context.FilterByValues(
            context.BigIntTable,
            table => table.Value,
            values,
            async table => await table.Select(t => t.Value).ToArrayAsync(),
            CancellationToken.None);
        Assert.True(values.OrderBy(_ => _)
            .SequenceEqual(foundValues.OrderBy(_ => _)));
    }

    #endregion

    #region string

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task FilterByStringValues(int filteredValuesCount)
    {
        await using var context = _fixture.BuildDataContext();
        var values = await GetValues<string, StringTable>(
            filteredValuesCount,
            context.StringTable);

        var foundItems = await context.FilterByValues(
            context.StringTable,
            table => table.Value,
            values,
            CancellationToken.None);
        var foundValues = foundItems
            .Select(table => table.Value)
            .ToArray();
        Assert.True(values.OrderBy(_ => _)
            .SequenceEqual(foundValues.OrderBy(_ => _)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task FilterByStringValuesWithResult(int filteredValuesCount)
    {
        await using var context = _fixture.BuildDataContext();
        var values = await GetValues<string, StringTable>(
            filteredValuesCount,
            context.StringTable);
        var foundValues = await context.FilterByValues(
            context.StringTable,
            table => table.Value,
            values,
            async table => await table.Select(t => t.Value).ToArrayAsync(),
            CancellationToken.None);
        Assert.True(values.OrderBy(_ => _)
            .SequenceEqual(foundValues.OrderBy(_ => _)));
    }

    #endregion

    #region guid

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task FilterByGuidValues(int filteredValuesCount)
    {
        await using var context = _fixture.BuildDataContext();
        var values = await GetValues<Guid, GuidTable>(
            filteredValuesCount,
            context.GuidTable);
        var foundItems = await context.FilterByValues(
            context.GuidTable,
            table => table.Value,
            values,
            CancellationToken.None);
        var foundValues = foundItems
            .Select(table => table.Value)
            .ToArray();
        Assert.True(values.OrderBy(_ => _)
            .SequenceEqual(foundValues.OrderBy(_ => _)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(1000)]
    public async Task FilterByGuidValuesWithResult(int filteredValuesCount)
    {
        await using var context = _fixture.BuildDataContext();
        var values = await GetValues<Guid, GuidTable>(
            filteredValuesCount,
            context.GuidTable);
        var foundValues = await context.FilterByValues(
            context.GuidTable,
            table => table.Value,
            values,
            async table => await table.Select(t => t.Value).ToArrayAsync(),
            CancellationToken.None);
        Assert.True(values.OrderBy(_ => _)
            .SequenceEqual(foundValues.OrderBy(_ => _)));
    }

    #endregion

    [Fact]
    public async Task FilterByEqualityOperatorIsParametrized()
    {
        await using var context = _fixture.BuildDataContext();
        const string value = "testValue";
        string? querySql = null;
        await ValueFilterExtensions.FilterByEqualityOperator(
            context.StringTable,
            t => t.Value,
            value,
            filteredQuery =>
            {
                querySql = filteredQuery.ToString();
                return Task.CompletedTask;
            });
        var selectSql = GetSelectSql(querySql);
        Assert.False(selectSql.Contains($"'{value}'", StringComparison.OrdinalIgnoreCase));
        Assert.True(selectSql.Contains(value: "= @p", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task FilterByContainsOperatorIsParametrized()
    {
        await using var context = _fixture.BuildDataContext();
        const string value = "testValue";
        string? querySql = null;
        await ValueFilterExtensions.FilterByContainsOperator(
            context.StringTable,
            t => t.Value,
            new [] {value},
            filteredQuery =>
            {
                querySql = filteredQuery.ToString();
                return Task.CompletedTask;
            });

        var selectSql = GetSelectSql(querySql);
        Assert.False(selectSql.Contains($"'{value}'", StringComparison.OrdinalIgnoreCase));
        Assert.True(selectSql.Contains(value: "IN (@p)", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task FilterByTempTableJoinHasNoNullComparison()
    {
        await using var context = _fixture.BuildDataContext();
        const string value = "testValue";
        string? querySql = null;
        await ValueFilterExtensions.FilterByTempTableJoin(
            context,
            context.StringTable,
            t => t.Value,
            new [] {value},
            filteredQuery =>
            {
                querySql = filteredQuery.ToString();
                return Task.CompletedTask;
            },
            CancellationToken.None);

        var selectSql = GetSelectSql(querySql);
        Assert.False(selectSql.Contains(value: "NULL", StringComparison.OrdinalIgnoreCase));
    }

    private static async Task<IReadOnlyList<TResult>> GetValues<TResult, TMappedModel>(
        int count,
        IQueryable<TMappedModel> table)
        where TMappedModel : TestTable<TResult>
    {
        var values = await table
            .Take(count)
            .Select(t => t.Value)
            .ToArrayAsync();

        Assert.Equal(count, values.Length);

        return values;
    }

    private static string GetSelectSql(string? query)
    {
        Assert.NotNull(query);

        var index = query!.IndexOf(value: "SELECT", StringComparison.OrdinalIgnoreCase);
        Assert.True(index >= 0);

        var selectSql = query.Substring(
            index,
            query.Length - index);
        Assert.False(string.IsNullOrWhiteSpace(selectSql));
        return selectSql;
    }
}