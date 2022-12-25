using System;
using DemoApplication.Areas.Client.ViewModels.Color;
using DemoApplication.Database;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.Controllers
{

    [Area("client")]
    [Route("basket")]

    public class ColorController : Controller
    {
        private readonly DataContext _dataContext;
        public ColorController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("add-color/{id}", Name = "add-color")]

        public async Task<IActionResult> AddColorAsync([FromRoute] int id)
        {
            var color = await _dataContext.Colors.FirstOrDefaultAsync(c => c.Id == id);

            if (color is null)
            {
                return NotFound();
            }

            var cookie = HttpContext.Request.Cookies["colors"];

            if (cookie is null)
            {
                var colorViewMdoel = new ColorViewModel(color.Id, color.Name);

                HttpContext.Response.Cookies.Append("colors", JsonSerializer.Serialize(colorViewMdoel));

            }

            else
            {
                var cokie = JsonSerializer.Deserialize<ColorViewModel>(cookie);
                var colorViewMdoel = new ColorViewModel(color.Id, color.Name);

                HttpContext.Response.Cookies.Append("colors", JsonSerializer.Serialize(colorViewMdoel));

            }

            return RedirectToRoute("client-home-index");
        }
    }
}