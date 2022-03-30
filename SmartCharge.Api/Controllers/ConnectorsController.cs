using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharge.Application.Features;
using SmartCharge.Application.Models;

namespace SmartCharge.Api.Controllers
{
    [Route("api/charge-stations/{chargeStationId}/connectors")]
    [ApiController]
    public class ConnectorsController : ControllerBase
    {
        private readonly IConnectorService _connectorService;

        public ConnectorsController(IConnectorService connectorService)
        {
            _connectorService = connectorService;
        }

        [HttpPost]
        public async Task<ActionResult<ConnectorDto>> CreateConnector(int chargeStationId, ConnectorForManipulationDto dto)
        {
            var connectorDto = await _connectorService.AddConnector(chargeStationId, dto);

            return CreatedAtRoute("GetConnectorForChargeStation",
                new { chargeStationId = chargeStationId, connectorId = dto.ConnectorId }, connectorDto);
        }


        [HttpGet("{connectorId}", Name = "GetConnectorForChargeStation")]
        public ActionResult<ConnectorDto> GetConnectorForChargeStation(int chargeStationId, int connectorId)
        {
            var connector = _connectorService.GetConnector(chargeStationId, connectorId);
            return Ok(connector);
        }

        [HttpPut("{connectorId}", Name = "UpdateConnectorForChargeStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateConnectorForChargeStation(int chargeStationId, int connectorId, ConnectorForManipulationDto connectorForManipulationDto)
        {
            await _connectorService.Update(chargeStationId, connectorId, connectorForManipulationDto);

            return Ok();

        }

        [HttpDelete("{connectorId}", Name = "DeleteConnectorForChargeStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int chargeStationId, int connectorId)
        {
            await _connectorService.Delete(chargeStationId, connectorId);
            return Ok();
        }


    }
}
