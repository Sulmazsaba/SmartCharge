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
    public class ConnectorRepository : RepositoryBase<Connector>, IConnectorRepository
    {
        private readonly SmartChargeContext smartChargeContext;
        public ConnectorRepository(SmartChargeContext dbContext) : base(dbContext)
        {
            smartChargeContext = dbContext;
        }

        public  int CountByChargeStationId(int chargeStationId)
        {
            return  smartChargeContext.Connectors.Count(i=>i.ChargeStationId==chargeStationId);
        }

        public Connector GetByKeys(int chargeStationId, int connectorId)
        {
           return smartChargeContext.Connectors.Where(i=>i.ChargeStationId==chargeStationId && i.Id ==connectorId).FirstOrDefault();    
        }
    }
}
