using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Expressions;
using LinqToDB.SqlQuery;

namespace Linq2db.Sql.Extensions.Optimization.Extensions;

[PublicAPI]
public static class SqlExtensions
{
    private const int MssqlMaxParamsInQuery = 2100;

    [LinqToDB.Sql.Extension(expression: "{field} IN ({values, ', '})", IsPredicate = true, BuilderType = typeof(ParametersBuilder))]
    public static bool In<T>(
        this LinqToDB.Sql.ISqlExtension ext,
        [ExprParameter] T field,
        [SqlQueryDependent] IEnumerable<T> values)
        => throw new NotSupportedException(); // this method is actually implemented by attribute and won't throw Exception

    /// <remarks><code>IReadOnlyCollection value</code> need to contain one element</remarks>
    [LinqToDB.Sql.Extension(expression: "{field} = {values}", IsPredicate = true, BuilderType = typeof(ParametersBuilder))]
    internal static bool Equals<T>(
        this LinqToDB.Sql.ISqlExtension ext,
        [ExprParameter] T field,
        [SqlQueryDependent] IEnumerable<T> values)
        => throw new NotSupportedException(); // this method is actually implemented by attribute and won't throw Exception

    private class ParametersBuilder : LinqToDB.Sql.IExtensionCallBuilder
    {
        public void Build(LinqToDB.Sql.ISqExtensionBuilder builder)
        {
            const string valuesName = "values";
            var values = builder
               .GetValue<System.Collections.IEnumerable>(valuesName)
               .OfType<object>()
               .ToArray();

            if (values.Length > 0)
            {
                var paramsCondition = values.Length > MssqlMaxParamsInQuery;
                foreach (var param in values.Select(value => BuildParameter(value, paramsCondition)))
                {
                    builder.AddParameter(valuesName, param);
                }
            }
            else
            {
                builder.AddParameter(valuesName, string.Empty);
            }
        }
    }

    private static ISqlExpression BuildParameter(
        object? value,
        bool paramsCondition)
    {
        const string parameterName = "p";
        var type = new DbDataType(value?.GetType() ?? typeof(object));
        var param = paramsCondition
            ? new SqlValue(type, value)
            : new SqlParameter(type, parameterName, value) as ISqlExpression;
        return param;
    }
}