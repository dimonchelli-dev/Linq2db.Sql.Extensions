using JetBrains.Annotations;
using LinqToDB.Mapping;

namespace Linq2db.Sql.Extensions.Optimization.Tests.Database.Tables;

[PublicAPI]
[Table(tableName: "BigIntTable")]
public class BigIntTable : TestTable<long>
{
}