using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Linq2db.Sql.Extensions.Optimization.Extensions;
using Linq2db.Sql.Extensions.Optimization.Models;
using Linq2db.Sql.Extensions.Optimization.Tests.CollectionFixtures;
using Linq2db.Sql.Extensions.Optimization.Tests.Database;
using LinqToDB;
using Xunit;

namespace Linq2db.Sql.Extensions.Optimization.Tests;

[Trait(name: "Category",value: "Integration")]
[Collection(FixtureCollectionNames.SqlDbCollection)]
public class TempTableExtensionsTests
{
    private readonly SqlDbFixture _fixture;

    public TempTableExtensionsTests(SqlDbFixture fixture)
        => _fixture = fixture;

    #region InsertIntIntoTemporaryTable

    [Theory]
    [InlineData(1, 100)]
    public void InsertIntIntoTemporaryTable(int start, int count)
    {
        var items = Enumerable.Range(start, count)
            .ToArray();
        InsertIntoTemporaryTable(items);
    }

    [Theory]
    [InlineData(1, 100)]
    public void InsertStringIntoTemporaryTable(int start, int count)
    {
        var items = Enumerable.Range(start, count)
            .Select(i => i.ToString())
            .ToArray();
        InsertIntoTemporaryTable(items);
    }

    [Theory]
    [InlineData(1, 100)]
    public void InsertLongIntoTemporaryTable(int start, int count)
    {
        var items = Enumerable.Range(start, count)
            .Select(i => long.MaxValue/2 + i)
            .ToArray();
        InsertIntoTemporaryTable(items);
    }

    [Theory]
    [InlineData(1, 100)]
    public void InsertGuidIntoTemporaryTable(int start, int count)
    {
        var items = Enumerable.Range(start, count)
            .Select(Int2Guid)
            .ToArray();
        InsertIntoTemporaryTable(items);
    }

    private void InsertIntoTemporaryTable<T>(IReadOnlyCollection<T> items)
    {
        using var context = _fixture.BuildDataContext();
#pragma warning disable CS0618
        var tempTable = context.InsertIntoTemporaryTable(
#pragma warning restore CS0618
            items.Select(item => new FieldValueTable<T> { Value = item }).ToArray(), customTempTableName: "customName");

        var storedItems = tempTable
            .Select(table => table.Value)
            .ToArray();

        try
        {
            Assert.True(items.OrderBy(_ => _)
                .SequenceEqual(storedItems.OrderBy(_ => _)));
        }
        finally
        {
            tempTable.Drop(throwExceptionIfNotExists: false);
        }
        Assert.True(items.SequenceEqual(storedItems));
    }

    #endregion

    #region InsertIntoTemporaryTableAsync

    [Theory]
    [InlineData(1, 100)]
    public async Task InsertIntIntoTemporaryTableAsync(int start, int count)
    {
        var items = Enumerable.Range(start, count)
            .ToArray();
        await InsertIntoTemporaryTableAsync(items);
    }

    [Theory]
    [InlineData(1, 100)]
    public async Task InsertStringIntoTemporaryTableAsync(int start, int count)
    {
        var items = Enumerable.Range(start, count)
            .Select(i => i.ToString())
            .ToArray();
        await InsertIntoTemporaryTableAsync(items);
    }

    [Theory]
    [InlineData(1, 100)]
    public async Task InsertLongIntoTemporaryTableAsync(int start, int count)
    {
        var items = Enumerable.Range(start, count)
            .Select(i => long.MaxValue/2 + i)
            .ToArray();
        await InsertIntoTemporaryTableAsync(items);
    }

    [Theory]
    [InlineData(1, 100)]
    public async Task InsertGuidIntoTemporaryTableAsync(int start, int count)
    {
        var items = Enumerable.Range(start, count)
            .Select(Int2Guid)
            .ToArray();
        await InsertIntoTemporaryTableAsync(items);
    }

    private async Task InsertIntoTemporaryTableAsync<T>(IReadOnlyCollection<T> items)
    {
        await using var context = _fixture.BuildDataContext();
        var models = items
            .Distinct()
            .Select(item => new FieldValueTable<T> { Value = item })
            .ToArray();
        var tempTable = await context.InsertIntoTemporaryTableAsync(
            models, customTempTableName: "customName");

        var storedItems = tempTable
            .Select(table => table.Value)
            .ToArray();

        try
        {
            Assert.True(items.OrderBy(_ => _)
                .SequenceEqual(storedItems.OrderBy(_ => _)));
        }
        finally
        {
            await tempTable.DropAsync(throwExceptionIfNotExists: false);
        }
        Assert.True(items.SequenceEqual(storedItems));
    }

    #endregion

    #region IgnoreDoubledDataOnInsertIntoTemporaryTableAsync

    [Theory]
    [InlineData( new []{"a","b","B"}, new[] {"a", "b"})]
    [InlineData( new []{"c","c","C"}, new []{"c"})]
    public async Task IgnoreDoubledStringDataOnInsertIntoTemporaryTableAsync(string[] items, string[] expectedStoredItems)
    {
        await IgnoreDoubledDataOnInsertIntoTemporaryTableAsync(
            context => context.InsertIntoTemporaryTableAsync(items),
            expectedStoredItems,
            StringComparer.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData( new []{1,2,3,3}, new[] {1,2,3})]
    [InlineData( new []{3,3,3}, new []{3})]
    public async Task IgnoreDoubledIntDataOnInsertIntoTemporaryTableAsync(int[] items, int[] expectedStoredItems)
    {
        await IgnoreDoubledDataOnInsertIntoTemporaryTableAsync(
            context => context.InsertIntoTemporaryTableAsync(items),
            expectedStoredItems,
            EqualityComparer<int>.Default);
    }

    private async Task IgnoreDoubledDataOnInsertIntoTemporaryTableAsync<T>(
        Func<TestDataContext, Task<ITable<FieldValueTable<T>>>> tempTableFunc,
        IReadOnlyCollection<T> expectedStoredItems,
        IEqualityComparer<T> comparer)
    {
        await using var context = _fixture.BuildDataContext();
        var tempTable = await tempTableFunc(context);
        var storedItems = tempTable
            .Select(table => table.Value)
            .ToArray();

        try
        {
            Assert.True(expectedStoredItems.OrderBy(_ => _)
                .SequenceEqual(storedItems.OrderBy(_ => _),comparer));
        }
        finally
        {
            await tempTable.DropAsync(throwExceptionIfNotExists: false);
        }
    }

    #endregion

    #region IgnoreNullDataOnInsertIntoTemporaryTableAsync

    [Theory]
    [InlineData( new []{"a",null,"b"}, new[] {"a", "b"})]
    [InlineData( new []{"a",null,"b",null}, new[] {"a", "b"})]
    [InlineData( new string? []{null}, new string[0])]
    [InlineData( new string? []{null, null}, new string[0])]
    public async Task IgnoreNullDataOnInsertIntoTemporaryTableAsync(string?[] items, string[] expectedStoredItems)
    {
        await using var context = _fixture.BuildDataContext();
#pragma warning disable CS8620
        var tempTable = await context.InsertIntoTemporaryTableAsync(items);
#pragma warning restore CS8620
        var storedItems = tempTable
            .Select(table => table.Value)
            .ToArray();

        try
        {
            Assert.True(expectedStoredItems.OrderBy(_ => _)
                .SequenceEqual(storedItems.OrderBy(_ => _), StringComparer.OrdinalIgnoreCase));
        }
        finally
        {
            await tempTable.DropAsync(throwExceptionIfNotExists: false);
        }
    }

    #endregion

    #region WithTempTable

    [Theory]
    [InlineData(new [] {-666, 1},new [] {1})]
    public async Task WithTempTableFilter(int[] requested, int[] expected)
    {
        await using var context = _fixture.BuildDataContext();
        var found =await  context.WithTempTable(requested, async tempTable =>
        {
            var query = from storedValues in context.IntTable
                join tempValues in tempTable on storedValues.Value equals tempValues.Value
                select storedValues.Value;
            return await query.ToArrayAsync();
        });

        Assert.True(expected.OrderBy(_ => _)
            .SequenceEqual(found.OrderBy(_ => _)));
    }

    [Theory]
    [InlineData(5, 15)]
    public async Task WithTempTable(int start, int count)
    {
        var items = Enumerable.Range(start, count).ToArray();
        await using var context = _fixture.BuildDataContext();
        await context.WithTempTable(items, async tempTable =>
        {
            var query = tempTable
                .Where(table => table.Value % 2 == 0);
            await query.DeleteAsync();
            var restCount = await tempTable.CountAsync();
            var expectedRestCount = items.Count(item => item % 2 != 0);
            Assert.Equal(expectedRestCount, restCount);
        });
    }

    #endregion

    private static Guid Int2Guid(int value)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, index: 0);
        return new Guid(bytes);
    }
}