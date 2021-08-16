using AutoMapper;
using System.Collections.Generic;

namespace UTask.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDisplayInfo>();
        }
    }
}
