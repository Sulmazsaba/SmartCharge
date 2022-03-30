using AutoMapper;
using FluentAssertions;
using Moq;
using SmartCharge.Application.Contracts.Persistence;
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
    public class ChargeStationServiceTests
    {
        private  ChargeStationService chargeStationService;
        private ChargeStationForManipulationDto chargeStationForManipulationDto;
        private readonly Mock<IChargeStationRepository> chargeStationRepositoryMock;
        private IMapper _mapper;
        ChargeStation chargeStation;
        public ChargeStationServiceTests()
        {
            chargeStationForManipulationDto = new ChargeStationForManipulationDto()
            {
                Name = "Charge Station1"
            };

            chargeStation = new ChargeStation()
            {
                GroupId = 3,
                Name = "Charge Station2"
            };

            chargeStationRepositoryMock = new Mock<IChargeStationRepository>();


            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ChargeStationProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            chargeStationRepositoryMock.Setup(o => o.AddAsync(It.IsAny<ChargeStation>()));
            chargeStationRepositoryMock.Setup(o => o.Find(3,2)).Returns(chargeStation);
            chargeStationService = new ChargeStationService(_mapper, chargeStationRepositoryMock.Object);
        }

        [Fact]
        public async Task AddChargeStation_PassValidValue_ShouldBeSet()
        {
            ChargeStationDto result = await chargeStationService.AddChargeStation(1,chargeStationForManipulationDto);

            result.Id.Should().NotBe(null);
            result.Name.Should().Be(chargeStationForManipulationDto.Name);
        }

        [Fact]
        public async Task AddChargeStation_PassNull_ThrowException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => chargeStationService.AddChargeStation(2,null));

            exception.ParamName.Should().Be("chargeStationForManipulationDto");
        }

        [Fact]
        public async Task DeleteChargeStation_PassNull_ThrowException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => chargeStationService.DeleteChargeStation(2,2));

            exception.ParamName.Should().Be("chargeStationFromRepo");
        }

    }
}
