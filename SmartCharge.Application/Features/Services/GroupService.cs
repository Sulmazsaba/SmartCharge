using AutoMapper;
using SmartCharge.Application.Contracts.Persistence;
using SmartCharge.Application.Exceptions;
using SmartCharge.Application.Features;
using SmartCharge.Application.Models;
using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Features.Services
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IChargeStationRepository _chargeStationRepository;
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository, IMapper mapper,
            IChargeStationRepository chargeStationRepository)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
           _chargeStationRepository = chargeStationRepository;
        }

        public async Task<GroupDto> AddGroup(GroupForManipulationDto groupForManipulationDto)
        {
            AddOrUpdateValidation(groupForManipulationDto);

            var entity = _mapper.Map<Group>(groupForManipulationDto);

            await _groupRepository.AddAsync(entity);

            return _mapper.Map<GroupDto>(entity);
        }


        public async Task DeleteGroup(int id)
        {
            var groupFromRepo = await ValidateGroupExisted(id);
            await _groupRepository.DeleteAsync(groupFromRepo);
        }


        public async Task<GroupDto> GetGroup(int id)
        {
            var groupFromRepo = await ValidateGroupExisted(id);

            return _mapper.Map<GroupDto>(groupFromRepo);
        }
        public async Task UpdateGroup(int groupId, GroupForManipulationDto groupForManipulationDto)
        {
            AddOrUpdateValidation(groupForManipulationDto);


            var groupFromRepo = await ValidateGroupExisted(groupId);

            await ValidateCapacityInAmps(groupFromRepo);

            _mapper.Map(groupForManipulationDto, groupFromRepo);
            await _groupRepository.UpdateAsync(groupFromRepo);
        }


        private static void AddOrUpdateValidation(GroupForManipulationDto groupForManipulationDto)
        {
            if (groupForManipulationDto == null)
                throw new ArgumentNullException(nameof(groupForManipulationDto));

            if (groupForManipulationDto.CapacityInAmps <= 0)
                throw new BusinessException(ExceptionsMessages.CapacityInAmpsGreaterThanZero);
        }

        private async Task ValidateCapacityInAmps(Group group)
        {
            var allChargeStations = _chargeStationRepository.GetAllChargeStationsWithConnectors(group.Id);


            var sum = allChargeStations.Sum(o => o.Connectors.Sum(x => x.MaxCurrentInAmps));

            if (group.CapacityInAmps < sum)
            {
                throw new BusinessException(ExceptionsMessages.InvalidGroupCapacityInAmps);
            }
        }

        private async Task<Group> ValidateGroupExisted(int groupId)
        {
            var groupFromRepo = await _groupRepository.FindById(groupId);

            if (groupFromRepo == null)
                throw new ArgumentNullException(nameof(groupFromRepo));

            return groupFromRepo;
        }
    }
}
