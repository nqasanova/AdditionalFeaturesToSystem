using System;
namespace DemoApplication.Areas.Client.ViewModels.Account
{
    public class UpdateDetailsViewModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}