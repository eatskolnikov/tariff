using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServices.Repositories
{

    public class ProductsRepository : BaseRepository<Product>, IProductsRepository
    {
        public ProductsRepository(AppDbContext database) : base(database)
        {
        }
    }

    public interface IProductsRepository : IBaseRepository<Product>
    {

    }
}
