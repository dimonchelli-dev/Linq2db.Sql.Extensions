using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinqToDB.Data;

namespace Linq2db.Sql.Extensions.Optimization.Extensions;

[PublicAPI]
public static class TypedValueFilterExtensions
{
    #region int

    public static Task<IReadOnlyList<TMappedModel>> FilterByValues<TMappedModel>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, int>> itemFieldExpression,
        IReadOnlyCollection<int> fieldValues,
        CancellationToken cancellationToken)
        where TMappedModel : class =>
        connection.FilterByValues<TMappedModel, int>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            cancellationToken);

    public static Task FilterByValues<TMappedModel>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, int>> itemFieldExpression,
        IReadOnlyCollection<int> fieldValues,
        Func<IQueryable<TMappedModel>, Task> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class =>
        connection.FilterByValues<TMappedModel, int>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            filteredItemsTask,
            cancellationToken);

    public static Task<TResult> FilterByValues<TMappedModel, TResult>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, int>> itemFieldExpression,
        IReadOnlyCollection<int> fieldValues,
        Func<IQueryable<TMappedModel>, Task<TResult>> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class
        where TResult : class =>
        connection.FilterByValues<TMappedModel, int, TResult>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            filteredItemsTask,
            cancellationToken);

    #endregion

    #region long

    public static Task<IReadOnlyList<TMappedModel>> FilterByValues<TMappedModel>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, long>> itemFieldExpression,
        IReadOnlyCollection<long> fieldValues,
        CancellationToken cancellationToken)
        where TMappedModel : class =>
        connection.FilterByValues<TMappedModel, long>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            cancellationToken);

    public static Task FilterByValues<TMappedModel>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, long>> itemFieldExpression,
        IReadOnlyCollection<long> fieldValues,
        Func<IQueryable<TMappedModel>, Task> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class =>
        connection.FilterByValues<TMappedModel, long>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            filteredItemsTask,
            cancellationToken);

    public static Task<TResult> FilterByValues<TMappedModel, TResult>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, long>> itemFieldExpression,
        IReadOnlyCollection<long> fieldValues,
        Func<IQueryable<TMappedModel>, Task<TResult>> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class
        where TResult : class =>
        connection.FilterByValues<TMappedModel, long, TResult>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            filteredItemsTask,
            cancellationToken);

    #endregion

    #region string

    public static Task<IReadOnlyList<TMappedModel>> FilterByValues<TMappedModel>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, string>> itemFieldExpression,
        IReadOnlyCollection<string> fieldValues,
        CancellationToken cancellationToken)
        where TMappedModel : class =>
        connection.FilterByValues<TMappedModel, string>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            cancellationToken);

    public static Task FilterByValues<TMappedModel>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, string>> itemFieldExpression,
        IReadOnlyCollection<string> fieldValues,
        Func<IQueryable<TMappedModel>, Task> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class =>
        connection.FilterByValues<TMappedModel, string>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            filteredItemsTask,
            cancellationToken);

    public static Task<TResult> FilterByValues<TMappedModel, TResult>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, string>> itemFieldExpression,
        IReadOnlyCollection<string> fieldValues,
        Func<IQueryable<TMappedModel>, Task<TResult>> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class
        where TResult : class =>
        connection.FilterByValues<TMappedModel, string, TResult>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            filteredItemsTask,
            cancellationToken);

    #endregion

    #region guid

    public static Task<IReadOnlyList<TMappedModel>> FilterByValues<TMappedModel>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, Guid>> itemFieldExpression,
        IReadOnlyCollection<Guid> fieldValues,
        CancellationToken cancellationToken)
        where TMappedModel : class =>
        connection.FilterByValues<TMappedModel, Guid>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            cancellationToken);

    public static Task FilterByValues<TMappedModel>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, Guid>> itemFieldExpression,
        IReadOnlyCollection<Guid> fieldValues,
        Func<IQueryable<TMappedModel>, Task> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class =>
        connection.FilterByValues<TMappedModel, Guid>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            filteredItemsTask,
            cancellationToken);

    public static Task<TResult> FilterByValues<TMappedModel, TResult>(
        this DataConnection connection,
        IQueryable<TMappedModel> itemsQuery,
        Expression<Func<TMappedModel, Guid>> itemFieldExpression,
        IReadOnlyCollection<Guid> fieldValues,
        Func<IQueryable<TMappedModel>, Task<TResult>> filteredItemsTask,
        CancellationToken cancellationToken)
        where TMappedModel : class
        where TResult : class =>
        connection.FilterByValues<TMappedModel, Guid, TResult>(
            itemsQuery,
            itemFieldExpression,
            fieldValues,
            filteredItemsTask,
            cancellationToken);

    #endregion
}