using BlogEngine.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Api.ViewModels
{
    public class UserInputViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        internal static UserInputViewModel ToViewModel(User user)
        {
            var result = new UserInputViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return result;
        }

        internal static User ToModel(UserInputViewModel viewModel)
        {
            var result = new User
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
            };

            return result;
        }
    }
}
