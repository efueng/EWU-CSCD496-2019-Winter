using AutoMapper;
using BlogEngine.Api.ViewModels;
using BlogEngine.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Api.Models
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<UserInputViewModel, User>();
            CreateMap<User, UserInputViewModel>();

            CreateMap<User, UserViewModel>();
        }
    }
}
