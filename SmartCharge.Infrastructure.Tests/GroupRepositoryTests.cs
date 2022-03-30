using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartCharge.Domain.Entities;
using SmartCharge.Infrastructure.Persistence;
using SmartCharge.Infrastructure.Repositories;
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

            var result =await groupRepository.FindById(group.Id);

            result.Id.Should().NotBe(null);
            result.CapacityInAmps.Should().Be(group.CapacityInAmps);
            result.Name.Should().Be(group.Name);
        }

        [Fact]
        public async Task FindGroupById_WhenInvalidId_ShouldReturnNull()
        {
            var dbOptions = new DbContextOptionsBuilder<SmartChargeContext>().UseInMemoryDatabase("FindGroupByInvalidIdTest").Options;

            using var context = new SmartChargeContext(dbOptions);

            var groupRepository = new GroupRepository(context); var group = new Group
            {
                Id = 1,
                CapacityInAmps = 2,
                Name = "test"
            };

            await groupRepository.AddAsync(group);
            var result = await groupRepository.FindById(2);
            result.Should().BeNull();

        }

    }
}