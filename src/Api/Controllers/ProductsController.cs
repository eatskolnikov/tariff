using AppServices.Services;
using Domain.Entities;
using Domain.Framework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppServices.Framework.Extensions;
using Domain.Dto;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        IProductsService _productsService;
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public TaskResult<List<Product>> GetPage(int page=1, int lenght=10, string orderBy="id asc")
        {
            var result = new TaskResult<List<Product>>();
            try
            {
                var data = _productsService.DataSet()
                                              .Where(x=>x.IsActive)
                                              .Skip((page-1) * lenght)
                                              .Take(lenght).ToList();

                result.Data = data;
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            return result;
        }

        [HttpGet("{id}")]
        public TaskResult<Product> GetById(int? id)
        {
            var result = new TaskResult<Product>();
            try
            {
                if (id.HasValue)
                {
                    result.Data = _productsService.DataSet().First(x => x.Id == id.Value && x.IsActive);
                }
            }
            catch(Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            return result;
        }


        [HttpPost]
        public TaskResult<Product> Create(Product product)
        {
            var result = new TaskResult<Product>();
            try
            {
                result = _productsService.Create(product);
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            return result;
        }


        [HttpPut]
        public TaskResult<Product> Update(Product product)
        {
            var result = new TaskResult<Product>();
            try
            {
                result = _productsService.Update(product);
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            return result;
        }

        [HttpDelete("{id}")]
        public TaskResult Delete(int id)
        {
            var result = new TaskResult();
            try
            {
                result = _productsService.Delete(id);
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            return result;
        }

        [HttpGet("/Compare")]
        public TaskResult<List<TariffCostCalculation>> Compare(decimal yearlykwh)
        {
            var result = new TaskResult<List<TariffCostCalculation>>();
            try
            {
                var products = _productsService.DataSet(x => x.IsActive).ToList();
                var data = products.Select(x => _productsService.CalculateTarrifCost(x, yearlykwh))
                                    .OrderBy(x => x.AnualCost)
                                    .ToList();

                result.Data = data;
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            return result;
        }
    }
}
