using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartCharge.Domain.Entities;
using SmartCharge.Infrastructure.Persistence;
using SmartCharge.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SmartCharge.Infrastructure.Tests
{
    public class ConnectorRepositoryTests
    {
        private readonly Group _group;
        public ConnectorRepositoryTests()
        {
            _group = new Group
            {
                Id = 1,
                CapacityInAmps = 2,
                Name = "test",
                ChargeStations = new List<ChargeStation>()
                {
                   new ChargeStation()
                   {
                       Id=1,
                       Name = "Charge Station1",
                       Connectors = new List<Connector>()
                       {
                           new Connector()
                           {
                               Id=1,
                               MaxCurrentInAmps = 3
                           },new Connector()
                           {
                               Id=2,
                               MaxCurrentInAmps= 4
                           }
                       }
                   }, new ChargeStation()
                   {
                       Id=2,
                       Name = "Charge Station2",
                       Connectors = new List<Connector>()
                       {
                           new Connector()
                           {
                               Id=3,
                               MaxCurrentInAmps = 2
                           },new Connector()
                           {
                               Id=4,
                               MaxCurrentInAmps= 11
                           }
                       }
                   }
                }
            };

        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(3, 0)]
        public async Task GetCountByChargeStationId_shouldReturnExpectedValue(int chargeStationId, int expectedResult)
        {
            //Arrange

            Random rnd = new Random();
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase($"CountByChargeStationIdTest_{rnd.Next()}").Options;

            using var context = new SmartChargeContext(dbOptions);

            var connectorRepository = new ConnectorRepository(context);
            var groupRepository = new GroupRepository(context);


            //Act
            await groupRepository.AddAsync(_group);
            var result = connectorRepository.CountByChargeStationId(chargeStationId);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(2, 3)]
        [InlineData(1, 2)]
        [InlineData(1, 4)]
        [InlineData(3, 1)]
        public async Task GetConnectorsByKeys_ShouldReturnExpectedResult(int chargeStationId, int connectorId)
        {
            //Arrange

            Random rnd = new Random();
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase($"CountByChargeStationIdTest_{rnd.Next()}").Options;

            using var context = new SmartChargeContext(dbOptions);

            var connectorRepository = new ConnectorRepository(context);
            var groupRepository = new GroupRepository(context);

            //Act
            await groupRepository.AddAsync(_group);
            var result = connectorRepository.GetByKeys(chargeStationId, connectorId);

            //Assert

            Connector expectedResult = _group.ChargeStations.Where(i => i.Id == chargeStationId).Select(i => i.Connectors.FirstOrDefault(i => i.Id == connectorId)).FirstOrDefault();
            result.Should().Be(expectedResult);

        }

    }
}
