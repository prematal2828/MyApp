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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext=appDbContext;
        }

        public void Update(Category category)
        {
            var existingcategory = _appDbContext.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingcategory != null)
            {
                existingcategory.Name = category.Name;
                existingcategory.DisplayOrder = category.DisplayOrder;

            }
            //_appDbContext.Categories.Update(category);
        }
    }
}
