using System;
using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Client.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current password is required")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Entered passwords do not match!")]
        public string? ConfirmPassword { get; set; }
    }
}