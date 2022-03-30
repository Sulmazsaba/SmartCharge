using AutoMapper;
using SmartCharge.Application.Models;
using SmartCharge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Mapping
{
    public class ConnectorProfile : Profile
    {
        public ConnectorProfile()
        {
            CreateMap<ConnectorForManipulationDto,Connector>();
            CreateMap<Connector, ConnectorDto>();
        }
    }
}
