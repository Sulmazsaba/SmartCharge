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
    public class ChargeStationRepositoryTests
    {
        private readonly Group _group;
        public ChargeStationRepositoryTests()
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


        [Fact]
        public async Task GetSumMaxCurrentInAmpsConnectors_ShouldReturnCorrectValue()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("GetSumMaxCurrentInAmpsConnectorsTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var chargeStationRepository = new ChargeStationRepository(context);
            var groupRepository = new GroupRepository(context);
            await groupRepository.AddAsync(_group);


            var maxCurrentInAmps = chargeStationRepository.GetSumMaxCurrentInAmpsConnectors(_group.Id);
            maxCurrentInAmps.Should().Be(20);
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(3, false)]
        public async Task GetChargeStationById_ShouldReturnCorrectValue(int chargeStationId, bool expectedResult)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase($"GetChargeStationByIdTest_{chargeStationId}").Options;

            using var context = new SmartChargeContext(dbOptions);

            var chargeStationRepository = new ChargeStationRepository(context);
            var groupRepository = new GroupRepository(context);

            //Action
            await groupRepository.AddAsync(_group);
            var chargeStation = _group.ChargeStations.FirstOrDefault(i => i.Id == chargeStationId);

            var result = await chargeStationRepository.GetById(chargeStationId);

            // Assert
            if (!expectedResult)
                result.Should().Be(null);
            else
            {
                result.Id.Should().Be(chargeStation?.Id);
                result.Name.Should().Be(chargeStation?.Name);
            }
        }

        [Fact]
        public async Task GetAllChargeStationsWithConnectors_ShouldReturnCorrectList()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("GetAllChargeStationsWithConnectorsTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var chargeStationRepository = new ChargeStationRepository(context);
            var groupRepository = new GroupRepository(context);

            //Action
            await groupRepository.AddAsync(_group);
            var result = chargeStationRepository.GetAllChargeStationsWithConnectors(1);

            //Assert
            result.Should().Contain(i => i.GroupId == 1);
            result.Should().Contain(i => i.Connectors.Count() == 2);
            result.Should().Contain(i => i.Name == "Charge Station1");
        }

        [Theory]
        [InlineData(1,1,true)]
        [InlineData(1,3,false)]
        [InlineData(2,1,false)]
        public async Task GetByChargeStaionIdAndGroupId_ShouldReturnExpectedResult(int groupId,int chargeStationId,bool expectedResult)
        {

            //Arrange
            Random rnd = new Random();
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase($"GetByChargeStaionIdAnGroupId_{rnd.Next()}").Options;
            using var context = new SmartChargeContext(dbOptions);
            var chargeStationRepository = new ChargeStationRepository(context);
            var groupRepository = new GroupRepository(context);

            //Action
            await groupRepository.AddAsync(_group);
            var result = chargeStationRepository.Find(groupId, chargeStationId);

            //Assert
            if (!expectedResult)
            {
                result.Should().BeNull();
            }
            else
            {
                result.Should().Be(_group.ChargeStations.Where(i=>i.GroupId==groupId && i.Id==chargeStationId).FirstOrDefault());
            }
        }
    }
}
