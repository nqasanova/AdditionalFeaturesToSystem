using DemoApplication.Areas.Client.ViewModels.Authentication;
using DemoApplication.Database.Models;

namespace DemoApplication.Services.Abstracts
{
    public interface IUserService
    {
        public User CurrentUser { get; }
        public Task<bool> CheckUserAsync(string email, string password);

        public Task SignInAsync(Guid Id);
        public Task SignInAsync(string email, string password);
        public Task SignOutAsync();

        Task CreateAsync(RegisterViewModel model);
        public bool IsAuthenticated { get; }

        public string GetUserFullName();
    }
}