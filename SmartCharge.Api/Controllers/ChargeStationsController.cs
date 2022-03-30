using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharge.Application.Features;
using SmartCharge.Application.Models;

namespace SmartCharge.Api.Controllers
{
    [Route("api/groups/{groupId}/charge-stations")]
    [ApiController]
    public class ChargeStationsController : ControllerBase
    {

        private readonly IChargeStationService _chargeStationService;

        public ChargeStationsController(IChargeStationService chargeStationService)
        {
            _chargeStationService = chargeStationService;
        }

        [HttpGet("{chargeStationId}", Name = "GetChargeStationForGroup")]
        public async Task<ActionResult<ChargeStationDto>> GetChargeStationForGroup(int groupId, int chargeStationId)
        {
            var result = await _chargeStationService.GetChargeStationOfGroup(groupId, chargeStationId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ChargeStationDto>> CreateChargeStationForGroup(int groupId, ChargeStationForManipulationDto dto)
        {
            var chargeStationDto = await _chargeStationService.AddChargeStation(groupId, dto);

            return CreatedAtRoute("GetChargeStationForGroup",
                new { groupId = chargeStationDto.GroupId, chargeStationId = chargeStationDto.Id }, chargeStationDto);
        }

        [HttpPut("{chargeStationId}", Name = "UpdateChargeStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateChargeStationForGroup(int groupId, int chargeStationId, ChargeStationForManipulationDto chargeStationForManipulationDto)
        {
            await _chargeStationService.UpdateChargeStation(groupId, chargeStationId, chargeStationForManipulationDto);
            return NoContent();

        }

        [HttpDelete("{chargeStationId}", Name = "DeleteChargeStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int groupId, int chargeStationId)
        {
            await _chargeStationService.DeleteChargeStation(groupId, chargeStationId);
            return NoContent();
        }



    }
}
