using System;
using DemoApplication.Areas.Client.ViewModels.Home.Index;
using DemoApplication.Database;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("shop")]
    public class ShopController : Controller
    {
        private readonly DataContext _dataContext;

        public ShopController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet("index", Name = "client-shop-index")]
        public async Task<IActionResult> Index(int sort, int page = 1)
        {
            var model = new List<BookListItemViewModel>();
            switch (sort)
            {
                case 1:
                    model = await _dataContext.Books
                        .Skip((page - 1) * 4).Take(4)
                        .OrderBy(b => b.Title)
                        .Select(b => new BookListItemViewModel(b.Id, b.Title, $"{b.Author.FirstName} {b.Author.LastName}", b.Price))
                        .ToListAsync();
                    break;

                case 2:
                    model = await _dataContext.Books
                        .Skip((page - 1) * 4).Take(4)
                        .OrderByDescending(b => b.Title)
                        .Select(b => new BookListItemViewModel(b.Id, b.Title, $"{b.Author.FirstName} {b.Author.LastName}", b.Price))
                        .ToListAsync();
                    break;

                case 3:
                    model = await _dataContext.Books
                        .Skip((page - 1) * 4).Take(4)
                        .OrderBy(b => b.Price)
                        .Select(b => new BookListItemViewModel(b.Id, b.Title, $"{b.Author.FirstName} {b.Author.LastName}", b.Price))
                        .ToListAsync();
                    break;

                case 4:
                    model = await _dataContext.Books
                        .Skip((page - 1) * 4).Take(4)
                        .OrderByDescending(b => b.Price)
                        .Select(b => new BookListItemViewModel(b.Id, b.Title, $"{b.Author.FirstName} {b.Author.LastName}", b.Price))
                        .ToListAsync();
                    break;

                default:
                    model = await _dataContext.Books
                        .Skip((page - 1) * 4).Take(4)
                      .Select(b => new BookListItemViewModel(b.Id, b.Title, $"{b.Author.FirstName} {b.Author.LastName}", b.Price))
                      .ToListAsync();
                    break;
            }

            ViewBag.Page = 1;
            ViewBag.Total = _dataContext.Books.Count() / 4;

            return View(model);
        }
    }
}