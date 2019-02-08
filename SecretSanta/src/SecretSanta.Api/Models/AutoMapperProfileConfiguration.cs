using AutoMapper;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Api.Models
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<Gift, GiftViewModel>();
            
            // Ignores User.Id in lossy conversion to UserInputViewModel
            CreateMap<User, UserInputViewModel>()
                .ForSourceMember(member => member.Id, options => options.DoNotValidate());
            CreateMap<User, UserViewModel>();
            
            CreateMap<UserInputViewModel, User>();

            CreateMap<Group, GroupViewModel>();
            CreateMap<GroupInputViewModel, Group>();

            //CreateMap<GroupUser, GroupUserViewModel>();
        }
    }
}