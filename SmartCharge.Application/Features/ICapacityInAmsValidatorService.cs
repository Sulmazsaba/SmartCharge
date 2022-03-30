using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Features
{
    public interface ICapacityInAmsValidatorService
    {
        void ValidateCapacityInAms(int groupId);
    }
}
