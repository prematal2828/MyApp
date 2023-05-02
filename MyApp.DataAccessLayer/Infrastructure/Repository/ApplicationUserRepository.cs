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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public ApplicationUserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext=appDbContext;
        }

    }
}
