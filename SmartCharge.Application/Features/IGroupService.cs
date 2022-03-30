using SmartCharge.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Features
{
    public interface IGroupService
    {
        Task<GroupDto> AddGroup(GroupForManipulationDto groupForManipulationDto);
        Task<GroupDto> GetGroup(int id);

        Task DeleteGroup(int id);

        Task UpdateGroup(int groupId,GroupForManipulationDto groupForManipulationDto);

    }
}
