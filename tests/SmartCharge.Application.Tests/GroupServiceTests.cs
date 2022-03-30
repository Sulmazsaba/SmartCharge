using AutoMapper;
using FluentAssertions;
using Moq;
using SmartCharge.Application.Contracts.Persistence;
using SmartCharge.Application.Exceptions;
using SmartCharge.Application.Features;
using SmartCharge.Application.Features.Services;
using SmartCharge.Application.Mapping;
using SmartCharge.Application.Models;
using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SmartCharge.Application
{
    public class GroupServiceTests
    {
        private  GroupForManipulationDto _groupForManipulationDto;
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<IChargeStationRepository> _chargeStationRepositoryMock;
        private GroupService _groupService;
        private IMapper _mapper;
        Group group;
        List<ChargeStation> chargeStationList;

        public GroupServiceTests()
        {
            _groupForManipulationDto = new GroupForManipulationDto
            {
                CapacityInAmps = 10,
                Name = "Group 4"
            };

            group = new Group
            {
                CapacityInAmps = 2,
                Name = "test",
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


            _groupRepositoryMock = new Mock<IGroupRepository>();
            _chargeStationRepositoryMock = new Mock<IChargeStationRepository>();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new GroupsProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _groupRepositoryMock.Setup(o => o.AddAsync(It.IsAny<Group>()));
            _groupRepositoryMock.Setup(o => o.FindById(3)).Returns(Task.FromResult(group));
            _groupRepositoryMock.Setup(o => o.UpdateAsync(It.IsAny<Group>()));
            _chargeStationRepositoryMock.Setup(o => o.GetAllChargeStationsWithConnectors(It.IsAny<int>())).Returns(chargeStationList);

            _groupService = new GroupService(_groupRepositoryMock.Object, _mapper,_chargeStationRepositoryMock.Object);

        }

        [Fact]
        public async Task AddGroup_PassValidValue_ShouldBeSet()
        {

            GroupDto result = await _groupService.AddGroup(_groupForManipulationDto);


            result.Id.Should().NotBe(null);
            result.Name.Should().Be(_groupForManipulationDto.Name);
            result.CapacityInAmps.Should().Be(_groupForManipulationDto.CapacityInAmps);
        }

        [Fact]
        public async Task AddGroup_WhenInvalidCapacityInAmps_ThrowException()
        {
            _groupForManipulationDto = new GroupForManipulationDto
            {
                CapacityInAmps = 0,
                Name = "Group 4"
            };
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _groupService.AddGroup(_groupForManipulationDto));

            exception.Message.Should().Be(ExceptionsMessages.CapacityInAmpsGreaterThanZero);
        }

        [Fact]
        public async Task AddGroup_PassNullGroup_ThrowException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _groupService.AddGroup(null));

            exception.ParamName.Should().Be("groupForManipulationDto");
        }

        [Fact]
        public async Task DeleteGroup_WhenNotExistedGroup_ThrowException()
        {
            var action = (async () =>
             {
                 await _groupService.DeleteGroup(12);
             });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetGroup_WhenNotExistedGroup_ThrowException()
        {
            var action = (async () =>
            {
                await _groupService.GetGroup(12);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public  async Task GetGroup_PassValidValue_ShouldBeReturn()
        {
            GroupDto dto = await _groupService.GetGroup(3);


            dto.Name.Should().Be(group.Name);
            dto.Id.Should().Be(group.Id);
            dto.CapacityInAmps.Should().Be(group.CapacityInAmps);
        }

        [Fact]
        public async Task UpdateGroup_WhenNotExistedGroup_ThrowException()
        {
            var action = (async () =>
            {
                await _groupService.UpdateGroup(12,_groupForManipulationDto);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        [Fact]
        public async Task UpdateGroup_WhenInvalidCapacityInAmps_ThrowException()
        {
            group.CapacityInAmps = 29;
           

            var exception = await Assert.ThrowsAsync<BusinessException>(() => _groupService.UpdateGroup(3, _groupForManipulationDto));


            exception.Message.Should().Be(ExceptionsMessages.InvalidGroupCapacityInAmps);
        }


    }
}
