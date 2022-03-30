using AutoMapper;
using FluentAssertions;
using Moq;
using SmartCharge.Application.Contracts.Persistence;
using SmartCharge.Application.Exceptions;
using SmartCharge.Application.Features.Services;
using SmartCharge.Application.Mapping;
using SmartCharge.Application.Models;
using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SmartCharge.Application.Tests
{
    public class ConnectorServiceTests
    {
        private ConnectorService connectorService;
        private ConnectorForManipulationDto connectorForManipulationDto;
        private readonly Mock<IConnectorRepository> connectorRepositoryMock;
        private readonly Mock<IGroupRepository> groupRepositoryMock;
        private readonly Mock<IChargeStationRepository> chargeStationRepositoryMock;
        Group group;
        private IMapper _mapper;
        List<ChargeStation> chargeStationList;
        ChargeStation chargeStation;
        Connector connector;

        public ConnectorServiceTests()
        {
            connector = new Connector()
            {
                
            };
            connectorForManipulationDto = new ConnectorForManipulationDto()
            {
                ConnectorId = 2,
                MaxCurrentInAmps = 10
            };

            chargeStationList = new List<ChargeStation>()
            {
                new ChargeStation()
                {
                    Connectors = new List<Connector>()
                    {
                        new Connector()
                        {
                            ChargeStationId = 1,
                            MaxCurrentInAmps = 10,
                        },
                        new Connector()
                        {
                            ChargeStationId= 2,
                            MaxCurrentInAmps= 6,
                        }
                    }

                },new ChargeStation()
                {
                    Connectors = new List<Connector>()
                    {
                        new Connector()
                        {
                            ChargeStationId = 3,
                            MaxCurrentInAmps = 4,
                        },
                        new Connector()
                        {
                            ChargeStationId= 4,
                            MaxCurrentInAmps= 10,
                        }
                    },

                }
            };
            chargeStation = new ChargeStation()
            {
                GroupId = 4,
            };
            group = new Group()
            {
                CapacityInAmps = 100
            };
            connectorRepositoryMock = new Mock<IConnectorRepository>();
            chargeStationRepositoryMock = new Mock<IChargeStationRepository>();
            groupRepositoryMock = new Mock<IGroupRepository>();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ConnectorProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            connectorRepositoryMock.Setup(x => x.CountByChargeStationId(2)).Returns(6);
            chargeStationRepositoryMock.Setup(o => o.GetAllChargeStationsWithConnectors(It.IsAny<int>())).Returns(chargeStationList);
            chargeStationRepositoryMock.Setup(s => s.GetById(4)).Returns(chargeStation);
            groupRepositoryMock.Setup(o => o.FindById(4)).Returns(Task.FromResult(group));
            connectorRepositoryMock.Setup(x => x.GetByKeys(4, 3)).Returns(connector);

            connectorService = new ConnectorService(connectorRepositoryMock.Object, _mapper, chargeStationRepositoryMock.Object, groupRepositoryMock.Object);
        }


        [Fact]
        public async Task AddConnector_PassNullConnector_ThrowException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => connectorService.AddConnector(1, null));

            exception.ParamName.Should().Be("connectorForManipulationDto");
        }

        [Fact]
        public async Task AddConnector_MoreThanFiveForOneChargeStation_ThrowException()
        {
            var exception = await Assert.ThrowsAsync<BusinessException>(() => connectorService.AddConnector(2, connectorForManipulationDto));

            exception.Message.Should().Be(ExceptionsMessages.MaxConnectorsCount);
        }

        [Fact]
        public async Task AddConnector_InvalidConnectorId_ThrowException()
        {
            connectorForManipulationDto.ConnectorId = 6;
            var exception = await Assert.ThrowsAsync<BusinessException>(() => connectorService.AddConnector(1, connectorForManipulationDto));

            exception.Message.Should().Be(ExceptionsMessages.ConnectorIdOutOfRange);

        }

        [Fact]
        public async Task AddConnector_InvalidMaxCurrentAmps_ThrowException()
        {
            connectorForManipulationDto.MaxCurrentInAmps = 0;
            var exception = await Assert.ThrowsAsync<BusinessException>(() => connectorService.AddConnector(1, connectorForManipulationDto));

            exception.Message.Should().Be(ExceptionsMessages.MaxCurrentInAmpsGreaterThanZero);
        }


        [Fact]
        public async Task AddConnector_InvalidMaxCurrentInAmps_ThrowsException()
        {
            group.CapacityInAmps = 29;

            var exception = await Assert.ThrowsAsync<BusinessException>(() => connectorService.AddConnector(4, connectorForManipulationDto));

            exception.Message.Should().Be(ExceptionsMessages.InvalidGroupCapacityInAmps);

        }


        [Fact]
        public async Task UpdateConnector_InvalidMaxCurrentInAmps_ThrowsException()
        {
            group.CapacityInAmps = 29;

            var exception = await Assert.ThrowsAsync<BusinessException>(() => connectorService.Update(4, 3, connectorForManipulationDto));
            exception.Message.Should().Be(ExceptionsMessages.InvalidGroupCapacityInAmps);

        }

        [Fact]
        public async Task UpdateConnector_WhenNotExistedGroup_ThrowsException()
        {

            var action = (async () =>
            {
                await connectorService.Update(12, 5,connectorForManipulationDto);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();

        }


        [Fact]
        public async Task DeleteConnector_WhenNotExistedGroup_ThrowException()
        {
            var action = (async () =>
            {
                await connectorService.Delete(12, 13);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        [Fact]
        public  void GetConnector_WhenNotExistedGroup_ThrowException()
        {
            var action = (async () =>
            {
                 connectorService.GetConnector(12, 13);
            });

             action.Should().ThrowAsync<ArgumentNullException>();

        }

    }
}
