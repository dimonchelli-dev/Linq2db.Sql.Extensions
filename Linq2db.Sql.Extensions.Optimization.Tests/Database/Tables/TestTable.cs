using JetBrains.Annotations;
using LinqToDB.Mapping;
#pragma warning disable CS8618

namespace Linq2db.Sql.Extensions.Optimization.Tests.Database.Tables;

[PublicAPI]
public abstract class TestTable<T>
{
    [Column(columnName: "Id")]
    public int Id { get; init; }
    [Column(columnName: "Value")]
    public T Value { get; init; }
}