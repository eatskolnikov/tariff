using Domain.Framework;
using Migrations.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using ThinkingHome.Migrator.Framework;
using ThinkingHome.Migrator.Framework.Extensions;

namespace Migrations
{
    [Migration(202110241107)]
    public class _202110241107_added_initial_tables : Migration
    {
        public override void Apply()
        {
            var columns = new List<Column>();
            columns.AddRange(ColumnsConstants.CommonColumns);

            columns.Add(new Column("Name", DbType.String.WithSize(250), ColumnProperty.NotNull));
            columns.Add(new Column("IsFlatFee", DbType.Boolean, ColumnProperty.NotNull));
            columns.Add(new Column("BaseCostPerMonth", DbType.Decimal, ColumnProperty.NotNull));
            columns.Add(new Column("PricePerKwh", DbType.Decimal, ColumnProperty.NotNull));
            columns.Add(new Column("FlatFeeBase", DbType.Decimal, ColumnProperty.NotNull));
            columns.Add(new Column("FlatFeeLimitKwh", DbType.Decimal, ColumnProperty.NotNull));
            columns.Add(new Column("FlatFeePricePerKwhAboveLimit", DbType.Decimal, ColumnProperty.NotNull));

            Database.AddTable(TableNameConstants.Products, columns.ToArray());
        }
    }
}
