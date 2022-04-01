using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Contracts.Persistence
{
    public interface IChargeStationRepository : IAsyncRepository<ChargeStation>
    {
        int GetSumMaxCurrentInAmpsConnectors(int groupId);

        //ChargeStation GetById(int id);

        List<ChargeStation> GetAllChargeStationsWithConnectors(int groupId);

        ChargeStation Find(int groupId,int chargeStationId);
    }
}
