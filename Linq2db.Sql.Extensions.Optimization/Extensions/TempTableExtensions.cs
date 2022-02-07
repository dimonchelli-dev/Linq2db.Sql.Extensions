using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinqToDB;
using LinqToDB.Data;

namespace Linq2db.Sql.Extensions.Optimization.Extensions;

[PublicAPI]
public static class TempTableExtensions
{
    /// <typeparam name="TMappedModel">linq2db table mapping class</typeparam>
    [Obsolete(message: "To support non-async legacy code")]
    public static ITable<TMappedModel> InsertIntoTemporaryTable<TMappedModel>(
        this IDataContext connection,
        IEnumerable<TMappedModel> mappedModels,
        string? customTempTableName = null)
        where TMappedModel : class
    {
        if (mappedModels == null)
        {
            throw new ArgumentException(message: null, nameof(mappedModels));
        }

        var tempTableName = BuildTempTableName<TMappedModel>(customTempTableName);
        var tempTable = connection.CreateTable<TMappedModel>(
            tempTableName);
        var rows = mappedModels
           .Where(m => m != null)
           .Distinct()
           .ToArray();
        if (rows.Length == 0)
        {
            return tempTable;
        }

        tempTable.BulkCopy(rows);
        return tempTable;
    }

    /// <typeparam name="TMappedModel">linq2db table mapping class</typeparam>
    public static async Task<ITable<TMappedModel>> InsertIntoTemporaryTableAsync<TMappedModel>(
        this IDataContext connection,
        IEnumerable<TMappedModel> mappedModels,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        where TMappedModel : class
    {
        if (mappedModels == null)
        {
            throw new ArgumentException(message: null, nameof(mappedModels));
        }

        var tempTableName = BuildTempTableName<TMappedModel>(customTempTableName);
        var tempTable = await connection.CreateTableAsync<TMappedModel>(
                tempTableName,
                token: cancellationToken)
           .ConfigureAwait(continueOnCapturedContext: false);
        var rows = mappedModels
           .Where(m => m != null)
           .Distinct()
           .ToArray();
        if (rows.Length == 0)
        {
            return tempTable;
        }

        await tempTable.BulkCopyAsync(
                rows,
                cancellationToken)
           .ConfigureAwait(continueOnCapturedContext: false);
        return tempTable;
    }

    /// <typeparam name="TMappedModel">linq2db table mapping class</typeparam>
    public static async Task WithTempTable<TMappedModel>(
        this DataConnection connection,
        IEnumerable<TMappedModel> mappedModels,
        Func<ITable<TMappedModel>, Task> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        where TMappedModel : class
    {
        var tempTableName = BuildTempTableName<TMappedModel>(customTempTableName);
        var tempTable = await connection.InsertIntoTemporaryTableAsync(
                mappedModels,
                tempTableName,
                cancellationToken)
           .ConfigureAwait(continueOnCapturedContext: false);
        try
        {
            await taskFunc.Invoke(tempTable)
               .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            await tempTable.DropTableAsync(
                    throwExceptionIfNotExists: false,
                    token: cancellationToken)
               .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    /// <typeparam name="TMappedModel">linq2db table mapping class</typeparam>
    /// <typeparam name="TResult">result class</typeparam>
    public static async Task<TResult> WithTempTable<TMappedModel, TResult>(
        this DataConnection connection,
        IEnumerable<TMappedModel> mappedModels,
        Func<ITable<TMappedModel>, Task<TResult>> taskFunc,
        string? customTempTableName = null,
        CancellationToken cancellationToken = default)
        where TMappedModel : class
    {
        var tempTableName = BuildTempTableName<TMappedModel>(customTempTableName);
        var tempTable = await connection.InsertIntoTemporaryTableAsync(
                mappedModels,
                tempTableName,
                cancellationToken)
           .ConfigureAwait(continueOnCapturedContext: false);
        try
        {
            return await taskFunc.Invoke(tempTable)
               .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            await tempTable.DropTableAsync(
                    throwExceptionIfNotExists: false,
                    token: cancellationToken)
               .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private static string BuildTempTableName<T>(
        string? customName = null)
    {
        var candidate = string.IsNullOrWhiteSpace(customName)
            ? typeof(T).Name
            : customName!.Trim();
        return BuildTableName(candidate);
    }

    private static string BuildTableName(
        string candidate)
    {
        if (string.IsNullOrWhiteSpace(candidate))
        {
            throw new ArgumentNullException(nameof(candidate));
        }

        var formatted = candidate.Trim();
        const string tempTablePrefix = "#";
        return formatted.StartsWith(tempTablePrefix)
            ? formatted
            : tempTablePrefix + formatted;
    }
}