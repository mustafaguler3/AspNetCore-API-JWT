using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Entities;

namespace UdemyAuthServer.Service
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<UserApp, UserAppDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();//productdto dan product yada terside olabilir
        }
    }
}
