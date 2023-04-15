using MyApp.DataAccessLayer.Data;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
            Category = new CategoryRepository(_appDbContext);
            Product = new ProductRepository(_appDbContext);
        }
        

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
