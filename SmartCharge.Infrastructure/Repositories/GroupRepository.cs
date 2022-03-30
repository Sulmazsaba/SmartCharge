using SmartCharge.Application.Contracts.Persistence;
using SmartCharge.Domain.Entities;
using SmartCharge.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Infrastructure.Repositories
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        private readonly SmartChargeContext _context;
        public GroupRepository(SmartChargeContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Group> FindById(int id)
        {
            return await _context.FindAsync<Group>(id);
        }
    }
}
