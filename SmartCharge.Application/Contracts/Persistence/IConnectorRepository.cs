using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Contracts.Persistence
{
    public interface IConnectorRepository : IAsyncRepository<Connector> 
    {
        int CountByChargeStationId(int chargeStationId);

        Connector GetByKeys(int chargeStationId, int connectorId);
    }
}
