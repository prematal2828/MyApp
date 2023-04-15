using MyApp.DataAccessLayer.Data;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext=appDbContext;
        }

        public void Update(Product product)
        {
            var existingproduct = _appDbContext.Products.FirstOrDefault(c => c.Id == product.Id);
            if (existingproduct != null)
            {
                existingproduct.Name = product.Name;
                existingproduct.Price = product.Price;
                existingproduct.Description = product.Description;
                if(product.ImageUrl != null)
                {
                    existingproduct.ImageUrl = product.ImageUrl;
                }
            }
            _appDbContext.Products.Update(product);
        }
    }
}
