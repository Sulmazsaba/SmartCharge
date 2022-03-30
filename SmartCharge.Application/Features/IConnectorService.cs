using SmartCharge.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Features
{
    public interface IConnectorService
    {

        Task<ConnectorDto> AddConnector(int chargeStationId, ConnectorForManipulationDto connectorForManipulationDto);

        ConnectorDto GetConnector(int chargeStationId,int connectorId);

        Task Update(int chargeStationId, int connectorId, ConnectorForManipulationDto connectorForManipulationDto);
        Task Delete(int chargeStationId, int connectorId);

    }
}
