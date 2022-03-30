using FluentValidation;
using SmartCharge.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Validations
{
    public class GroupValidation : AbstractValidator<GroupForManipulationDto>
    {
        public GroupValidation()
        {
            RuleFor(x => x.CapacityInAmps).GreaterThan(10).WithMessage("{CapacityInAmps} should be greater than zero");
        }

    }
}
