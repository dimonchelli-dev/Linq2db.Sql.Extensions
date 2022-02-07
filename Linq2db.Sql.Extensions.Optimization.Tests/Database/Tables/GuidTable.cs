using System;
using JetBrains.Annotations;
using LinqToDB.Mapping;

namespace Linq2db.Sql.Extensions.Optimization.Tests.Database.Tables;

[PublicAPI]
[Table(tableName: "GuidTable")]
public class GuidTable : TestTable<Guid>
{
}