using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppServices;
using AppServices.Repositories;
using AppServices.Services;
using Domain.Entities;
using EntityFrameworkCoreMock;
using Moq;
using NUnit.Framework;

namespace UnitTests.AppServices.Services
{
    [TestFixture]
    public class ProductsServiceTests
    {
        private IProductsService _sut;
        [SetUp]
        public void Setup()
        {
            var productsRepositoryMock = new Mock<IProductsRepository>();

            _sut = new ProductsService(productsRepositoryMock.Object);
        }

        [Test(Description = "Basic consumption plan, should return correct values")]
        public void calculating_basic_consumption_plan_return()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Basic electricity tariff",
                IsFlatFee = false,
                BaseCostPerMonth = 5,
                PricePerKwh = 0.22m,
                FlatFeeBase = 0,
                FlatFeeLimitKwh = 0,
                FlatFeePricePerKwhAboveLimit = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var result1 = _sut.CalculateTarrifCost(product, 3500);
            var result2 = _sut.CalculateTarrifCost(product, 4500);
            var result3 = _sut.CalculateTarrifCost(product, 6000);

            Assert.AreEqual(result1.ProductId, 1);
            Assert.AreEqual(result1.AnualCost, 830m);
            Assert.AreEqual(result1.TariffName, "Basic electricity tariff");

            Assert.AreEqual(result2.ProductId, 1);
            Assert.AreEqual(result2.AnualCost, 1050m);
            Assert.AreEqual(result2.TariffName, "Basic electricity tariff");

            Assert.AreEqual(result3.ProductId, 1);
            Assert.AreEqual(result3.AnualCost, 1380m);
            Assert.AreEqual(result3.TariffName, "Basic electricity tariff");
        }

        [Test(Description = "Packaged tarrif calculation below kwh limit")]
        public void calculating_packaged_tariff_below_kwh_limit()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Packaged tariff",
                IsFlatFee = true,
                BaseCostPerMonth = 0m,
                PricePerKwh = 0m,
                FlatFeeBase = 800m,
                FlatFeeLimitKwh = 4000m,
                FlatFeePricePerKwhAboveLimit = 0.30m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var result1 = _sut.CalculateTarrifCost(product, 3500);

            Assert.AreEqual(result1.ProductId, 1);
            Assert.AreEqual(result1.AnualCost, 800m);
            Assert.AreEqual(result1.TariffName, "Packaged tariff");
        }

        [Test(Description = "Packaged tarrif calculation above kwh limit")]
        public void calculating_packaged_tariff_above_kwh_limit_should_be_correct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Packaged tariff",
                IsFlatFee = true,
                BaseCostPerMonth = 0m,
                PricePerKwh = 0m,
                FlatFeeBase = 800m,
                FlatFeeLimitKwh = 4000m,
                FlatFeePricePerKwhAboveLimit = 0.30m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var result = _sut.CalculateTarrifCost(product, 4500);

            Assert.AreEqual(result.ProductId, 1);
            Assert.AreEqual(result.AnualCost, 950m);
            Assert.AreEqual(result.TariffName, "Packaged tariff");
        }

        [Test(Description = "Inserting a product with a long name")]
        public void inserting_a_product_with_a_long_name_should_fail()
        {
            var product = new Product
            {
                Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer accumsan porta turpis, at euismod odio iaculis at. Curabitur ut elit eros. Vestibulum ipsum nisi, eleifend eu enim sed, blandit finibus dolor. Aliquam erat volutpat. Sed facilisis purus auctor, dapibus dolor a, scelerisque erat.",
                IsFlatFee = true,
                BaseCostPerMonth = 0m,
                PricePerKwh = 0m,
                FlatFeeBase = 800m,
                FlatFeeLimitKwh = 4000m,
                FlatFeePricePerKwhAboveLimit = 0.30m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var result = _sut.Create(product);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Message, "The product name length most not exceed 250 characters");
        }


        [Test(Description = "Inserting a product with empty name")]
        public void inserting_a_product_with_empty_name_should_fail()
        {
            var product = new Product
            {
                Name =  string.Empty,
                IsFlatFee = true,
                BaseCostPerMonth = 0m,
                PricePerKwh = 0m,
                FlatFeeBase = 800m,
                FlatFeeLimitKwh = 4000m,
                FlatFeePricePerKwhAboveLimit = 0.30m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var result = _sut.Create(product);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Message, "The product name field is required");
        }

        [Test(Description = "Updating a product to have a long name")]
        public void updating_a_product_with_a_long_name_should_fail()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer accumsan porta turpis, at euismod odio iaculis at. Curabitur ut elit eros. Vestibulum ipsum nisi, eleifend eu enim sed, blandit finibus dolor. Aliquam erat volutpat. Sed facilisis purus auctor, dapibus dolor a, scelerisque erat.",
                IsFlatFee = true,
                BaseCostPerMonth = 0m,
                PricePerKwh = 0m,
                FlatFeeBase = 800m,
                FlatFeeLimitKwh = 4000m,
                FlatFeePricePerKwhAboveLimit = 0.30m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var result = _sut.Update(product);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Message, "The product name length most not exceed 250 characters");
        }

        [Test(Description = "Updating a product with empty name")]
        public void updating_a_product_with_empty_name_should_fail()
        {
            var product = new Product
            {
                Id = 1,
                Name = string.Empty,
                IsFlatFee = true,
                BaseCostPerMonth = 0m,
                PricePerKwh = 0m,
                FlatFeeBase = 800m,
                FlatFeeLimitKwh = 4000m,
                FlatFeePricePerKwhAboveLimit = 0.30m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var result = _sut.Update(product);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Message, "The product name field is required");
        }

        [Test(Description = "Updating a product with no id")]
        public void updating_a_product_with_no_id_should_fail()
        {
            var product = new Product
            {
                Name = "A big test",
                IsFlatFee = true,
                BaseCostPerMonth = 0m,
                PricePerKwh = 0m,
                FlatFeeBase = 800m,
                FlatFeeLimitKwh = 4000m,
                FlatFeePricePerKwhAboveLimit = 0.30m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var result = _sut.Update(product);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Message, "The id field is required");
        }
    }
}