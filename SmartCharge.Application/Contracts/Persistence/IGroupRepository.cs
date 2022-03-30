using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Contracts.Persistence
{
    public interface IGroupRepository : IAsyncRepository<Group>
    {
        Task<Group> FindById(int id);
    }
}
