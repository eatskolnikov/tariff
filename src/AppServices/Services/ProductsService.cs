using System;
using AppServices.Repositories;
using Domain.Dto;
using Domain.Entities;
using Domain.Framework;

namespace AppServices.Services
{
    public class ProductsService : BaseService<Product, IProductsRepository>, IProductsService
    {
        public ProductsService(IProductsRepository mainRepository) : base(mainRepository)
        {
        }

        public TariffCostCalculation CalculateTarrifCost(Product product, decimal consumption)
        {
            var result = new TariffCostCalculation
            {
                ProductId = product.Id,
                TariffName = product.Name
            };

            if (product.IsFlatFee)
            {
                result.AnualCost = product.FlatFeeBase;
                if (consumption > product.FlatFeeLimitKwh)
                {
                    result.AnualCost += (consumption - product.FlatFeeLimitKwh) * product.FlatFeePricePerKwhAboveLimit;
                }
            }
            else
            {
                result.AnualCost = (product.BaseCostPerMonth*12m) + (consumption * product.PricePerKwh);
            }
            return result;
        }

        protected override TaskResult<Product> ValidateOnCreate(Product entity)
        {
            return CommonValidations(entity);
        }

        protected override TaskResult ValidateOnDelete(Product entity)
        {
            return new TaskResult();
        }

        protected override TaskResult<Product> ValidateOnUpdate(Product entity)
        {
            var result = CommonValidations(entity);
            if (entity.Id <= 0)
            {
                result.AddErrorMessage("The id field is required");
            }
            return result;
        }

        private TaskResult<Product> CommonValidations(Product entity)
        {
            var result = new TaskResult<Product>();
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                result.AddErrorMessage("The product name field is required");
            }
            else if (entity.Name.Length > 250)
            {
                result.AddErrorMessage("The product name length most not exceed 250 characters");
            }
            return result;
        }
    }

    public interface IProductsService : IBaseService<Product, IProductsRepository>
    {
        TariffCostCalculation CalculateTarrifCost(Product product, decimal consumption);
    }
}
