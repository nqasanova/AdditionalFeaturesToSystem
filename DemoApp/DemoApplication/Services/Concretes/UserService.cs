using DemoApplication.Areas.Client.ViewModels.Authentication;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Contracts.Identity;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Exceptions;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using BC = BCrypt.Net.BCrypt;

namespace DemoApplication.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasketService _basketService;
        private User? _currentUSer;

        public UserService(DataContext dataContext, IHttpContextAccessor httpContextAccessor, IBasketService basketService)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _basketService = basketService;
        }

        public bool IsAuthenticated
        {
            get => _httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
        }

        public string GetUserFullName()
        {
            return $"{CurrentUser.FirstName} {CurrentUser.LastName}";
        }

        public User CurrentUser
        {
            get
            {
                if (_currentUSer is not null)
                {
                    return _currentUSer;
                }

                var idClaim = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(C => C.Type == CustomClaimNames.Id);

                if (idClaim == null)
                {
                    throw new IdentityCookieException("Identity cookie is not found");
                }

                return _dataContext.Users.First(u => u.Id == Guid.Parse(idClaim.Value));
            }
        }

        public async Task<bool> CheckUserAsync(string email, string password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user is not null && user.Password == password || BC.Verify(password, user.Password);
        }

        public async Task SignInAsync(Guid Id)
        {
            var claims = new List<Claim>
            {
                new Claim(CustomClaimNames.Id , Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task SignInAsync(string email, string password)
        {
            var user = await _dataContext.Users.FirstAsync(u => u.Email == email);

            if (user != null && BC.Verify(password, user.Password))
            {
                await SignInAsync(user.Id);
            }
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task CreateAsync(RegisterViewModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = BC.HashPassword(model.Password),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _dataContext.Users.AddAsync(user);

            var basket = new Basket
            {
                User = user,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _dataContext.Baskets.AddAsync(basket);

            var cookie = _httpContextAccessor.HttpContext.Request.Cookies["products"];

            var productsCookieViewModels = new List<ProductCookieViewModel>();

            if (cookie is not null)
            {
                productsCookieViewModels = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(cookie);

                foreach (var productCookieViewModels in productsCookieViewModels)
                {
                    var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == productCookieViewModels.Id);

                    var basketProduct = new BasketProduct
                    {
                        Basket = basket,
                        BookId = book.Id,
                        Quantity = productCookieViewModels.Quantity,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    await _dataContext.BasketProducts.AddAsync(basketProduct);
                }
            }
            await _dataContext.SaveChangesAsync();
        }
    }
}