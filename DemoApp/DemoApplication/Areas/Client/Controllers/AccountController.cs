using DemoApplication.Areas.Client.ViewModels.Account;
using DemoApplication.Areas.Client.ViewModels.Account.Address;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;


namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;

        public AccountController(DataContext dataContext, IUserService userService)
        {
            _dataContext = dataContext;
            _userService = userService;
        }

        #region Dashboard

        [HttpGet("dashboard", Name = "client-account-dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var user = _userService.CurrentUser;

            return View();
        }

        #endregion

        #region Orders

        [HttpGet("orders", Name = "client-account-orders")]
        public async Task<IActionResult> Orders()
        {
            var user = _userService.CurrentUser;

            return View();
        }

        #endregion

        #region Download

        [HttpGet("download", Name = "client-account-download")]
        public async Task<IActionResult> Download()
        {
            var user = _userService.CurrentUser;

            return View();
        }

        #endregion

        #region Payment

        [HttpGet("payment", Name = "client-account-payment")]
        public async Task<IActionResult> Payment()
        {
            var user = _userService.CurrentUser;

            return View();
        }

        #endregion

        #region Address

        [HttpGet("address", Name = "client-account-address")]
        public async Task<IActionResult> Address()
        {
            var user = _userService.CurrentUser;

            var address = await _dataContext.Addresses.FirstOrDefaultAsync(a => a.UserId == user.Id);

            if (address is null)
            {
                return View(new ItemViewModel());
            }

            var model = new ItemViewModel
            {
                OrdererFirstName = address.OrdererFirstName,
                OrdererLastName = address.OrdererLastName,
                PhoneNumber = address.PhoneNumber,
                Name = address.Name
            };

            return View(model);
        }

        #endregion

        #region Add Address

        [HttpGet("add-address", Name = "client-account-add-address")]
        public async Task<IActionResult> AddAddressAsync()
        {
            return View();
        }

        [HttpPost("add-address", Name = "client-account-add-address")]
        public async Task<IActionResult> AddAddressAsync(AddAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userService.CurrentUser;

            var address = new Address
            {
                OrdererFirstName = model.OrdererFirstName,
                OrdererLastName = model.OrdererLastName,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                UserId = user.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _dataContext.Addresses.AddAsync(address);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("client-account-address");
        }

        #endregion

        #region Update Address

        [HttpGet("update-address", Name = "client-account-update-address")]
        public async Task<IActionResult> UpdateAddressAsync()
        {
            var user = _userService.CurrentUser;
            var adreess = await _dataContext.Addresses.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (adreess == null)
            {
                return NotFound();
            }

            var addressViewModel = new UpdateAddressViewModel
            {
                OrdererFirstName = adreess.OrdererFirstName,
                OrdererLastName = adreess.OrdererLastName,
                PhoneNumber = adreess.PhoneNumber,
                Name = adreess.Name,
            };

            return View(addressViewModel);
        }

        [HttpPost("update-address", Name = "client-account-update-address")]
        public async Task<IActionResult> UpdateAddressAsync(UpdateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userService.CurrentUser;
            var adreess = await _dataContext.Addresses.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (adreess is null)
            {
                return NotFound();
            }
            adreess.OrdererFirstName = model.OrdererFirstName;
            adreess.OrdererLastName = model.OrdererLastName;
            adreess.PhoneNumber = model.PhoneNumber;
            adreess.Name = model.Name;

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("client-account-address");
        }

        #endregion

        #region Details

        [HttpGet("details", Name = "client-account-details")]
        public async Task<IActionResult> Details()
        {
            var user = _userService.CurrentUser;

            var model = new UpdateDetailsViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(model);
        }

        [HttpPost("details", Name = "client-account-details")]

        public async Task<IActionResult> Details(UpdateDetailsViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userService.CurrentUser;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("client-account-details");
        }

        #endregion

        #region Password

        [HttpGet("changepassword", Name = "client-account-changepassword")]
        public async Task<IActionResult> ChangePassword()
        {

            return View();
        }

        [HttpPost("changepassword", Name = "client-account-changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.CurrentUser;

            if (!BC.Verify(model.CurrentPassword, user.Password))
            {
                ModelState.AddModelError(String.Empty, "Your current password is not correct");
                return View(model);
            }

            user.Password = BC.HashPassword(model.Password);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("client-account-changepassword");
        }

        #endregion
    }
}