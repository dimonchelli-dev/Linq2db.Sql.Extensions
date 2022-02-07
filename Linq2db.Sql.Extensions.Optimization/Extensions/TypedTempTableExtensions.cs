using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Linq2db.Sql.Extensions.Optimization.Models;
using LinqToDB;
using LinqToDB.Data;

namespace Linq2db.Sql.Extensions.Optimization.Extensions;

[PublicAPI]
public static class TypedTempTableExtensions
{
    #region int

    public static Task<ITable<FieldValueTable<int>>> InsertIntoTemporaryTableAsync(
        this IDataContext connection,
        IEnumerable<int> values,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.InsertIntoTemporaryTableAsync(
            BuildFieldValueTableRows(values),
            customTempTableName,
            cancellationToken);

    [Obsolete(message: "To support non-async legacy code")]
    public static ITable<FieldValueTable<int>> InsertIntoTemporaryTable(
        this IDataContext connection,
        IEnumerable<int> values,
        string? customTempTableName = null)
        => connection.InsertIntoTemporaryTable(
            BuildFieldValueTableRows(values),
            customTempTableName);

    public static Task WithTempTable(
        this DataConnection connection,
        IEnumerable<int> values,
        Func<ITable<FieldValueTable<int>>, Task> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.WithTempTable(
            BuildFieldValueTableRows(values),
            taskFunc,
            customTempTableName,
            cancellationToken);

    public static Task<TResult> WithTempTable<TResult>(
        this DataConnection connection,
        IEnumerable<int> values,
        Func<ITable<FieldValueTable<int>>, Task<TResult>> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.WithTempTable(
            BuildFieldValueTableRows(values),
            taskFunc,
            customTempTableName,
            cancellationToken);

    #endregion

    #region long

    public static Task<ITable<FieldValueTable<long>>> InsertIntoTemporaryTableAsync(
        this IDataContext connection,
        IEnumerable<long> values,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.InsertIntoTemporaryTableAsync(
            BuildFieldValueTableRows(values),
            customTempTableName,
            cancellationToken);


    [Obsolete(message: "To support non-async legacy code")]
    public static ITable<FieldValueTable<long>> InsertIntoTemporaryTable(
        this IDataContext connection,
        IEnumerable<long> values,
        string? customTempTableName = null)
        => connection.InsertIntoTemporaryTable(
            BuildFieldValueTableRows(values),
            customTempTableName);

    public static Task WithTempTable(
        this DataConnection connection,
        IEnumerable<long> values,
        Func<ITable<FieldValueTable<long>>, Task> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.WithTempTable(
            BuildFieldValueTableRows(values),
            taskFunc,
            customTempTableName,
            cancellationToken);

    public static Task<TResult> WithTempTable<TResult>(
        this DataConnection connection,
        IEnumerable<long> values,
        Func<ITable<FieldValueTable<long>>, Task<TResult>> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.WithTempTable(
            BuildFieldValueTableRows(values),
            taskFunc,
            customTempTableName,
            cancellationToken);

    #endregion

    #region string

    public static Task<ITable<FieldValueTable<string>>> InsertIntoTemporaryTableAsync(
        this IDataContext connection,
        IEnumerable<string> values,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.InsertIntoTemporaryTableAsync(
            BuildFieldValueTableRows(values.Where(v => v != null), StringComparer.OrdinalIgnoreCase),
            customTempTableName,
            cancellationToken);

    [Obsolete(message: "To support non-async legacy code")]
    public static ITable<FieldValueTable<string>> InsertIntoTemporaryTable(
        this IDataContext connection,
        IEnumerable<string> values,
        string? customTempTableName = null)
        => connection.InsertIntoTemporaryTable(
            BuildFieldValueTableRows(values, StringComparer.OrdinalIgnoreCase),
            customTempTableName);

    public static Task WithTempTable(
        this DataConnection connection,
        IEnumerable<string> values,
        Func<ITable<FieldValueTable<string>>, Task> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.WithTempTable(
            BuildFieldValueTableRows(values, StringComparer.OrdinalIgnoreCase),
            taskFunc,
            customTempTableName,
            cancellationToken);

    public static Task<TResult> WithTempTable<TResult>(
        this DataConnection connection,
        IEnumerable<string> values,
        Func<ITable<FieldValueTable<string>>, Task<TResult>> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.WithTempTable(
            BuildFieldValueTableRows(values, StringComparer.OrdinalIgnoreCase),
            taskFunc,
            customTempTableName,
            cancellationToken);

    #endregion

    #region guid

    public static Task<ITable<FieldValueTable<Guid>>> InsertIntoTemporaryTableAsync(
        this IDataContext connection,
        IEnumerable<Guid> values,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.InsertIntoTemporaryTableAsync(
            BuildFieldValueTableRows(values),
            customTempTableName,
            cancellationToken);

    [Obsolete(message: "To support non-async legacy code")]
    public static ITable<FieldValueTable<Guid>> InsertIntoTemporaryTable(
        this IDataContext connection,
        IEnumerable<Guid> values,
        string? customTempTableName = null)
        => connection.InsertIntoTemporaryTable(
            BuildFieldValueTableRows(values),
            customTempTableName);

    public static Task WithTempTable(
        this DataConnection connection,
        IEnumerable<Guid> values,
        Func<ITable<FieldValueTable<Guid>>, Task> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.WithTempTable(
            BuildFieldValueTableRows(values),
            taskFunc,
            customTempTableName,
            cancellationToken);


    public static Task<TResult> WithTempTable<TResult>(
        this DataConnection connection,
        IEnumerable<Guid> values,
        Func<ITable<FieldValueTable<Guid>>, Task<TResult>> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        => connection.WithTempTable(
            BuildFieldValueTableRows(values),
            taskFunc,
            customTempTableName,
            cancellationToken);

    #endregion

    private static IReadOnlyCollection<FieldValueTable<T>> BuildFieldValueTableRows<T>(
        IEnumerable<T> values,
        IEqualityComparer<T>? comparer = null)
        => values
           .Distinct(comparer ?? EqualityComparer<T>.Default)
           .Select(v => new FieldValueTable<T> { Value = v })
           .ToArray();
}