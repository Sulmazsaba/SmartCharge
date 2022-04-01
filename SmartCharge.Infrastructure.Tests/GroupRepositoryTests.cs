using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartCharge.Domain.Entities;
using SmartCharge.Infrastructure.Persistence;
using SmartCharge.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SmartCharge.Infrastructure.Tests
{
    public class GroupRepositoryTests
    {

        [Fact]
        public async Task FindGroupById_ShouldReturnCorrectValue()
        {

            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("FindGroupByIdTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var groupRepository = new GroupRepository(context);

            var group = new Group
            {
                Id = 1,
                CapacityInAmps = 2,
                Name = "test"
            };

            await groupRepository.AddAsync(group);

            var result = await groupRepository.FindById(group.Id);

            result.Id.Should().NotBe(null);
            result.CapacityInAmps.Should().Be(group.CapacityInAmps);
            result.Name.Should().Be(group.Name);
        }

        [Fact]
        public async Task FindGroupById_WhenInvalidId_ShouldReturnNull()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("FindGroupByInvalidIdTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var groupRepository = new GroupRepository(context);
            var group = new Group
            {
                Id = 1,
                CapacityInAmps = 2,
                Name = "test"
            };

            await groupRepository.AddAsync(group);
            var result = await groupRepository.FindById(2);
            result.Should().BeNull();

        }


        [Fact]
        public async Task DeleteGroup_ShouldWorkCorrect()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("DeleteGroupTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var groupRepository = new GroupRepository(context);


            var group = new Group
            {
                Id = 1,
                CapacityInAmps = 2,
                Name = "test"
            };

            await groupRepository.AddAsync(group);
            await groupRepository.DeleteAsync(group);

            var result = await groupRepository.FindById(1);
            result.Should().BeNull();

        }

        [Fact]
        public async Task UpdateGroup_ShouldWorkCorrect()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("UpdateGroupTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var groupRepository = new GroupRepository(context);


            var group = new Group
            {
                Id = 1,
                CapacityInAmps = 2,
                Name = "test"
            };

            await groupRepository.AddAsync(group);

            group.Name = "test 1";
            await groupRepository.UpdateAsync(group);

            var result = await groupRepository.FindById(1);

            // Assert
            result.Name.Should().Be("test 1");
            result.Id.Should().Be(1);
            result.CapacityInAmps.Should().Be(2);

        }

        [Fact]
        public async Task DeleteGroup_ShouldDeleteIncludedChargeStations()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("CascadeDeleteGroupTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var groupRepository = new GroupRepository(context);
            var chargeStationRepository = new ChargeStationRepository(context);

            var group = new Group
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

                   }, new ChargeStation()
                   {
                       Id=2,
                       Name = "Charge Station2",

                   }
                }
            };

            await groupRepository.AddAsync(group);
            await groupRepository.DeleteAsync(group);

            var result =await chargeStationRepository.GetById(1);

            result.Should().BeNull();
        }

    }
}