using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkingHome.Migrator.Framework;

namespace Migrations.Framework
{
    public class ColumnsConstants
    {
        public static Column[] CommonColumns = {
            new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
            new Column("UpdatedAt",DbType.DateTime, ColumnProperty.NotNull),
            new Column("DeletedAt", DbType.DateTime, ColumnProperty.Null),
            new Column("CreatedAt", DbType.DateTime, ColumnProperty.NotNull),
            new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
        };
    }
}
