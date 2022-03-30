using AutoMapper;
using SmartCharge.Application.Contracts.Persistence;
using SmartCharge.Application.Exceptions;
using SmartCharge.Application.Models;
using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Features.Services
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IMapper _mapper;
        private readonly IChargeStationRepository _chargeStationRepository;

        public ChargeStationService(IMapper mapper, IChargeStationRepository chargeStationRepository)
        {
            _mapper = mapper;
            _chargeStationRepository = chargeStationRepository;
        }

        public async Task<ChargeStationDto> AddChargeStation(int groupId, ChargeStationForManipulationDto chargeStationForManipulationDto)
        {
            if(chargeStationForManipulationDto==null)
                throw new ArgumentNullException(nameof(chargeStationForManipulationDto));


            var entity = _mapper.Map<ChargeStation>(chargeStationForManipulationDto);
            entity.GroupId = groupId;

            await _chargeStationRepository.AddAsync(entity);


            return _mapper.Map<ChargeStationDto>(entity);
        }

        public async Task DeleteChargeStation(int chargeStationId, int groupId)
        {
            var chargeStationFromRepo = ValidateChargeStationExisted(groupId, chargeStationId);

            await _chargeStationRepository.DeleteAsync(chargeStationFromRepo);
        }

        public async Task<ChargeStationDto> GetChargeStationOfGroup(int groupId, int chargeStationId)
        {
            var chargeStationFromRepo = ValidateChargeStationExisted(groupId, chargeStationId);

            return _mapper.Map<ChargeStationDto>(chargeStationFromRepo);
           
        }


        public async Task UpdateChargeStation(int groupId, int chargeStationId, ChargeStationForManipulationDto dto)
        {
            var chargeStationFromRepo = ValidateChargeStationExisted(groupId, chargeStationId);

            _mapper.Map(dto, chargeStationFromRepo);
            await _chargeStationRepository.UpdateAsync(chargeStationFromRepo);
        }


        private  ChargeStation ValidateChargeStationExisted(int groupId , int chargeStationId)
        {
            var chargeStationFromRepo =  _chargeStationRepository.Find(groupId,chargeStationId);

            if (chargeStationFromRepo == null)
                throw new ArgumentNullException(nameof(chargeStationFromRepo));

            return chargeStationFromRepo;
        }
    }
}
