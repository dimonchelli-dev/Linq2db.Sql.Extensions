using JetBrains.Annotations;
using LinqToDB.Mapping;
#pragma warning disable CS8618

namespace Linq2db.Sql.Extensions.Optimization.Models;

[PublicAPI]
public class FieldValueTable<T>
{
    [Column(columnName: "Value")]
    [LinqToDB.Mapping.NotNull]
    public T Value { get; set; }
}