using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Linq2db.Sql.Extensions.Optimization.Models;
using LinqToDB;
using LinqToDB.Data;

[assembly: InternalsVisibleTo(assemblyName: "Linq2db.Sql.Extensions.Optimization.Tests")]

namespace Linq2db.Sql.Extensions.Optimization.Extensions;

internal static class ValueFilterExtensions
{
    internal static async Task<IReadOnlyList<TMappedModel>> FilterByValues<TMappedModel, TValue>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, TValue>> itemFieldExpression,
        IReadOnlyCollection<TValue> fieldValues,
        CancellationToken cancellationToken)
        where TMappedModel : class
    {
        return await FilterByValues<TMappedModel, TValue, IReadOnlyList<TMappedModel>>(
            connection,
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            async query =>
                await query.ToArrayAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false),
            cancellationToken);
    }

    internal static async Task FilterByValues<TMappedModel, TValue>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, TValue>> itemFieldExpression,
        IReadOnlyCollection<TValue> fieldValues,
        Func<IQueryable<TMappedModel>,Task> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class
    {
        if (fieldValues.Count == 0)
        {
            await filteredItemsTask(Array.Empty<TMappedModel>().AsQueryable());
            return;
        }

        var values = fieldValues
           .Distinct()
           .ToArray();

        if (values.Length == 1)
        {
            await FilterByEqualityOperator(
                    itemsQuery,
                    itemFieldExpression,
                    values[0],
                    filteredItemsTask)
               .ConfigureAwait(continueOnCapturedContext: false);
            return;
        }

        const int parametersMaxCount = 20;
        if (values.Length <= parametersMaxCount)
        {
            await FilterByContainsOperator(
                    itemsQuery,
                    itemFieldExpression,
                    fieldValues,
                    filteredItemsTask)
               .ConfigureAwait(continueOnCapturedContext: false);
            return;
        }

        await FilterByTempTableJoin(
                connection,
                itemsQuery,
                itemFieldExpression,
                fieldValues,
                filteredItemsTask,
                cancellationToken)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    internal static async Task<TResult> FilterByValues<TMappedModel, TValue, TResult>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, TValue>> itemFieldExpression,
        IReadOnlyCollection<TValue> fieldValues,
        Func<IQueryable<TMappedModel>, Task<TResult>> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class
        where TResult : class
    {
        TResult? result = null;
        async Task ResultTask(IQueryable<TMappedModel> query)
        {
            result = await filteredItemsTask(query)
               .ConfigureAwait(continueOnCapturedContext: false);
        }

        await FilterByValues(
                connection,
                itemsQuery,
                itemFieldExpression,
                fieldValues,
                ResultTask,
                cancellationToken)
           .ConfigureAwait(continueOnCapturedContext: false);

        return result ?? throw new InvalidOperationException(message: "Operation result is null");
    }

    internal static async Task FilterByEqualityOperator<TMappedModel, TValue>(
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, TValue>> itemFieldExpression,
        TValue fieldValue,
        Func<IQueryable<TMappedModel>, Task> filteredItemsTask)
    {
        var filteredQuery = itemsQuery
           .Where(x => LinqToDB.Sql.Ext!.Equals(itemFieldExpression.Compile()(x), new [] {fieldValue}));
        await filteredItemsTask(filteredQuery)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    internal static async Task FilterByContainsOperator<TMappedModel, TValue>(
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, TValue>> itemFieldExpression,
        IReadOnlyCollection<TValue> fieldValues,
        Func<IQueryable<TMappedModel>, Task> filteredItemsTask)
    {
        var filteredQuery = itemsQuery
           .Where(x => LinqToDB.Sql.Ext!.In(itemFieldExpression.Compile()(x), fieldValues));
        await filteredItemsTask(filteredQuery)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    internal static async Task FilterByTempTableJoin<TMappedModel, TValue>(
        DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, TValue>> itemFieldExpression,
        IEnumerable<TValue> fieldValues,
        Func<IQueryable<TMappedModel>, Task> filteredItemsTask,
        CancellationToken cancellationToken)
    {
        await connection
           .WithTempTable(
                fieldValues.Select(id => new FieldValueTable<TValue> { Value = id }),
                async idsTable =>
                {
                    var filteredQuery = itemsQuery
                       .Join(idsTable, itemFieldExpression, arg => arg.Value, (item, _) => item);
                    await filteredItemsTask(filteredQuery)
                       .ConfigureAwait(continueOnCapturedContext: false);
                },
                customTempTableName: "filter",
                cancellationToken)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

}