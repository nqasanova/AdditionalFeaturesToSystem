using System;
using DemoApplication.Areas.Client.ViewModels.Authentication;
using DemoApplication.Database;
using FluentValidation;

namespace DemoApplication.Areas.Client.Validations.User.Register
{
    public class RegisterViewModelValidation : AbstractValidator<RegisterViewModel>
    {

        private readonly DataContext _dataContext;
        public RegisterViewModelValidation(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(u => u.Email).Must(IsEmailUnique).WithMessage("Email is already in use!");
        }

        private bool IsEmailUnique(string email)
        {
            return !_dataContext.Users.Any(u => u.Email == email);
        }
    }
}