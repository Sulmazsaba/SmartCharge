using SmartCharge.Application.Models;
using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Features
{
    public interface IChargeStationService
    {
        Task<ChargeStationDto> AddChargeStation(int groupId, ChargeStationForManipulationDto chargeStationForManipulationDto);
        Task<ChargeStationDto> GetChargeStationOfGroup(int groupId,int chargeStationId);
        Task UpdateChargeStation(int groupId, int chargeStationId, ChargeStationForManipulationDto chargeStationForManipulation);
        Task DeleteChargeStation(int chargeStationId, int groupId);

    }
}
