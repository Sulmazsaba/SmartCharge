using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using SmartCharge.Application.Exceptions;
using SmartCharge.Application.Features;
using SmartCharge.Application.Models;
using System.Net;

namespace SmartCharge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<ActionResult<GroupDto>> CreateGroup(GroupForManipulationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var groupToReturn = await _groupService.AddGroup(dto);
            return CreatedAtRoute("GetGroup", new { groupId = groupToReturn.Id }, groupToReturn);
        }


        [HttpGet("{groupId}", Name = "GetGroup")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GroupDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GroupDto>> GetGroup(int groupId)
        {
            var result = await _groupService.GetGroup(groupId);
            return Ok(result);
        }

        [HttpDelete("{groupId}", Name = "DeleteGroup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteGroup(int groupId)
        {
            await _groupService.DeleteGroup(groupId);
            return NoContent();
        }

        [HttpPut("{groupId}", Name = "UpdateGroup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateGroup(int groupId, GroupForManipulationDto groupForManipulationDto)
        {
            await _groupService.UpdateGroup(groupId, groupForManipulationDto);
            return NoContent();
        }


        

    }
}
