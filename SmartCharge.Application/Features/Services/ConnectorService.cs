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
    public class ConnectorService : IConnectorService
    {
        private readonly IConnectorRepository _connectorRepository;
        private readonly IMapper _mapper;
        private readonly IChargeStationRepository chargeStationRepository;
        private readonly IGroupRepository groupRepository;

        public ConnectorService(IConnectorRepository connectorRepository, IMapper mapper, IChargeStationRepository chargeStationRepository, IGroupRepository groupRepository)
        {
            _connectorRepository = connectorRepository;
            _mapper = mapper;
            this.chargeStationRepository = chargeStationRepository;
            this.groupRepository = groupRepository;
        }

        public async Task<ConnectorDto> AddConnector(int chargeStationId, ConnectorForManipulationDto connectorForManipulationDto)
        {
            IsValidAddOrUpdateConnector(chargeStationId, connectorForManipulationDto);
            await ValidateMaxCurrentInAmpsInAddConnector(chargeStationId, connectorForManipulationDto);

            var entity = _mapper.Map<Connector>(connectorForManipulationDto);
            entity.ChargeStationId = chargeStationId;
            entity.Id = connectorForManipulationDto.ConnectorId;

            await _connectorRepository.AddAsync(entity);

            return _mapper.Map<ConnectorDto>(entity);
        }

        public async Task Delete(int chargeStationId, int connectorId)
        {
            var connectorFromRepo = ValidateConnectorExisted(chargeStationId, connectorId);

            await _connectorRepository.DeleteAsync(connectorFromRepo);
        }

        public ConnectorDto GetConnector(int chargeStationId, int connectorId)
        {
            var connectorFromRepo = ValidateConnectorExisted(chargeStationId, connectorId);

            return _mapper.Map<ConnectorDto>(connectorFromRepo);
        }

        public async Task Update(int chargeStationId, int connectorId, ConnectorForManipulationDto connectorForManipulationDto)
        {
            connectorForManipulationDto.ConnectorId = connectorId;
            IsValidAddOrUpdateConnector(chargeStationId, connectorForManipulationDto);


            var connectorFromRepo = ValidateConnectorExisted(chargeStationId, connectorId);
            await ValidateMaxCurrentInAmpsInUpdateConnector(chargeStationId, connectorForManipulationDto);


            _mapper.Map(connectorForManipulationDto, connectorFromRepo);
            await _connectorRepository.UpdateAsync(connectorFromRepo);
        }

        private void IsValidAddOrUpdateConnector(int chargeStationId, ConnectorForManipulationDto connectorForManipulationDto)
        {
            if (connectorForManipulationDto == null)
                throw new ArgumentNullException(nameof(connectorForManipulationDto));

            if (_connectorRepository.CountByChargeStationId(chargeStationId) >= 5)
                throw new BusinessException(ExceptionsMessages.MaxConnectorsCount);

            if (connectorForManipulationDto.ConnectorId < 0 || connectorForManipulationDto.ConnectorId > 5)
                throw new BusinessException(ExceptionsMessages.ConnectorIdOutOfRange);

            if (connectorForManipulationDto.MaxCurrentInAmps <= 0)
                throw new BusinessException(ExceptionsMessages.MaxCurrentInAmpsGreaterThanZero);
        }

        private async Task ValidateMaxCurrentInAmpsInAddConnector(int chargeStatationId, ConnectorForManipulationDto connectorForManipulationDto)
        {
            var chargeStation = chargeStationRepository.GetById(chargeStatationId);

            var group = await groupRepository.FindById(chargeStation.GroupId);

            var allChargeStations = chargeStationRepository.GetAllChargeStationsWithConnectors(chargeStation.GroupId);


            var sum = allChargeStations.Sum(o => o.Connectors.Sum(x => x.MaxCurrentInAmps));
            sum += connectorForManipulationDto.MaxCurrentInAmps;

            if (group.CapacityInAmps < sum)
            {
                throw new BusinessException(ExceptionsMessages.InvalidGroupCapacityInAmps);
            }
        }

        private async Task ValidateMaxCurrentInAmpsInUpdateConnector(int chargeStatationId, ConnectorForManipulationDto connectorForManipulationDto)
        {
            var chargeStation = chargeStationRepository.GetById(chargeStatationId);

            var group = await groupRepository.FindById(chargeStation.GroupId);

            var allChargeStations = chargeStationRepository.GetAllChargeStationsWithConnectors(chargeStation.GroupId);

            foreach (var item in allChargeStations)
            {
                if (item.Id == chargeStatationId)
                {
                    foreach (var connector in item.Connectors)
                    {
                        if (connector.Id == connectorForManipulationDto.ConnectorId)
                        {
                            connector.MaxCurrentInAmps = connectorForManipulationDto.MaxCurrentInAmps;
                        }
                    }
                }
            }


            var sum = allChargeStations.Sum(o => o.Connectors.Sum(x => x.MaxCurrentInAmps));

            if (group.CapacityInAmps < sum)
            {
                throw new BusinessException(ExceptionsMessages.InvalidGroupCapacityInAmps);
            }
        }

        private Connector ValidateConnectorExisted(int chargeStationId, int connectorId)
        {
            var connectorFromRepo = _connectorRepository.GetByKeys(chargeStationId, connectorId);
            if (connectorFromRepo == null)
                throw new ArgumentNullException(nameof(connectorFromRepo));

            return connectorFromRepo;
        }
    }
}
