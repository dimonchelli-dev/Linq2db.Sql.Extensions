using JetBrains.Annotations;
using LinqToDB.Mapping;

namespace Linq2db.Sql.Extensions.Optimization.Tests.Database.Tables;

[PublicAPI]
[Table(tableName: "IntTable")]
public class IntTable : TestTable<int>
{
}