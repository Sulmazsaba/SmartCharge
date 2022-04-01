using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using SmartCharge.Api.Controllers;
using SmartCharge.Application.Features;
using SmartCharge.Application.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SmartCharge.Api.Tests
{
    public class GroupsControllerTests
    {
        private readonly Mock<IGroupService> _groupService;
        private readonly GroupsController _groupsController;
        private readonly GroupForManipulationDto _dto;
        private readonly GroupDto _groupDto;
        public GroupsControllerTests()
        {
            _groupService = new Mock<IGroupService>();
            _groupDto = new GroupDto();
            _dto = new GroupForManipulationDto();

            _groupsController = new GroupsController(_groupService.Object);
        }


        [Theory]
        [InlineData(0, false, typeof(BadRequestObjectResult))]
        [InlineData(1, true, typeof(CreatedAtRouteResult))]
        public async Task CreateGroup_ShouldCallAddMethodWhenValid(int expectedMethodCalls, bool isModelValid, Type expectedActionResultType)
        {
            //Arrange
            _groupService.Setup(i => i.AddGroup(_dto)).ReturnsAsync(_groupDto);

            if (!isModelValid)
                _groupsController.ModelState.AddModelError("key", "ErrorMessage");

            //Action
            ActionResult<GroupDto> actionResult = await _groupsController.CreateGroup(_dto);


            //Assert
            actionResult.Result.ShouldBeOfType(expectedActionResultType);
            _groupService.Verify(i => i.AddGroup(_dto), Times.Exactly(expectedMethodCalls));
        }

        [Theory]
        [InlineData(true, typeof(OkObjectResult))]
        [InlineData(false, typeof(NotFoundResult))]
        public async Task GetGroup_ShouldCallGetGroupMethod(bool isExistedData, Type expectedActionResult)
        {
            //Arrange
            if (isExistedData)
                _groupService.Setup(i => i.GetGroup(It.IsAny<int>())).ReturnsAsync(_groupDto);
            else
                _groupService.Setup(i => i.GetGroup(It.IsAny<int>())).Throws(new ArgumentNullException());


            //Act
            //var result = await Assert.ThrowsAsync<ArgumentNullException>(() => _groupsController.GetGroup(2));
            //Assert.Equal(401, (int)result.);
            var actionResult = await _groupsController.GetGroup(2);

            //Assert
            actionResult.Result.ShouldBeOfType(expectedActionResult);
            _groupService.Verify(i => i.GetGroup(2), Times.Exactly(1));


        }

    }
}
