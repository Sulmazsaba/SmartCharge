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

        [Fact]
        public async Task GetChargeStationById_ShouldReturnCorrectValue()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("GetChargeStationByIdTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var chargeStationRepository = new ChargeStationRepository(context);
            var groupRepository = new GroupRepository(context);
            await groupRepository.AddAsync(_group);

            ChargeStation chargeStation = _group.ChargeStations.FirstOrDefault(i =>i.Id == 2);

            var result =await chargeStationRepository.GetById(2);
            result.Id.Should().Be(chargeStation?.Id);
            result.Name.Should().Be(chargeStation?.Name);
            
            chargeStation = _group.ChargeStations.FirstOrDefault(i => i.Id == 3);
            result =await chargeStationRepository.GetById(3);
            result.Should().Be(null);

        }
    }
}
