using Microsoft.EntityFrameworkCore;
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
    public class ChargeStationRepository : RepositoryBase<ChargeStation>, IChargeStationRepository
    {
        private readonly SmartChargeContext context;
        public ChargeStationRepository(SmartChargeContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }

        public int GetSumMaxCurrentInAmpsConnectors(int groupId)
        {
            return context.ChargeStations.Where(i => i.GroupId == groupId).Include(i => i.Connectors).Sum(i => i.Connectors.Sum(j => j.MaxCurrentInAmps));
        }

        public ChargeStation GetById(int id)
        {
            return context.Find<ChargeStation>(id);
        }

        public List<ChargeStation> GetAllChargeStationsWithConnectors(int groupId)
        {
            return context.ChargeStations.Where(i => i.GroupId == groupId).Include(i => i.Connectors).ToList();
        }

        public ChargeStation Find(int groupId, int chargeStationId)
        {
           return context.ChargeStations.FirstOrDefault(i=>i.GroupId == groupId &&i.Id == chargeStationId);
        }
    }
}
