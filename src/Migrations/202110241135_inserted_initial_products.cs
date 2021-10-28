using Domain.Framework;
using Migrations.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using ThinkingHome.Migrator.Framework;
using ThinkingHome.Migrator.Framework.Extensions;

namespace Migrations
{
    [Migration(202110241135)]
    public class _202110241135_inserted_initial_products : Migration
    {
        public override void Apply()
        {
            Database.Insert(new SchemaQualifiedObjectName { Name = TableNameConstants.Products }, new
            {
                Name = "Basic electricity tariff",
                IsFlatFee = 0,
                BaseCostPerMonth = 5m,
                PricePerKwh = 0.22m,
                FlatFeeBase = 0m,
                FlatFeeLimitKwh = 0m,
                FlatFeePricePerKwhAboveLimit = 0m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            Database.Insert(new SchemaQualifiedObjectName { Name = TableNameConstants.Products }, new
            {
                Name = "Packaged tariff",
                IsFlatFee = 1,
                BaseCostPerMonth = 0m,
                PricePerKwh = 0m,
                FlatFeeBase = 800m,
                FlatFeeLimitKwh = 4000m,
                FlatFeePricePerKwhAboveLimit = 0.30m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }
    }
}
