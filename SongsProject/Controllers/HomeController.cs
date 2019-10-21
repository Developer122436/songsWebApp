using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SongsProject.Infrastructure;
using SongsProject.Models;
using SongsProject.Models.ViewModels;
using System.Linq;

namespace SongsProject.Controllers
{
    public class HomeController : Controller
    {
        public int PageSize = 4;
        private readonly ISongRepository _repository;
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        // Constructor injection
        // Using dependency injection will easier to change code
        // We will change only startup method to change classes
        public HomeController(IHostingEnvironment hostingEnvironment, ISongRepository repo, ApplicationDbContext db, 
            ILogger<HomeController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _repository = repo;
            _db = db;
            _logger = logger;
        }

        public ViewResult ListCountry(string Country, int songPage = 1)
        {
            HttpContext.Session.Set("SongProject", new byte[0]);
            return View(new SongsListViewModel
            {
                Songs = _repository.Songs
                    .Where(p => Country == null || p.Country == Country)
                    .OrderByDescending(p => p.Rating)
                    //Skip - Skip on the table rows
                    .Skip((songPage - 1) * PageSize)
                    //Take - Take number of rows
                    .Take(PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = songPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Country == null
                        ? _repository.Songs.Count()
                        : _repository.Songs.Count(e => e.Country == Country)
                },
                CurrentCountry = Country
            });
        }

        [Route("Home/ListNames/{Name?}")]
        public ViewResult ListNames(string Name, int songPage = 1)
        {
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Warning Log");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");

            HttpContext.Session.Set("SongProject", new byte[0]);
            return View(new SongsListViewModel
            {
                Songs = _repository.Songs
                    .Where(p => Name == null || p.Name == Name)
                    .OrderByDescending(p => p.Rating)
                    //Skip - Skip on the table rows
                    .Skip((songPage - 1) * PageSize)
                    //Take - Take number of rows
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = songPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Name == null
                        ? _repository.Songs.Count()
                        : _repository.Songs.Count(e => e.Name == Name)
                },
                CurrentName = Name
            });
        }

        [Authorize(Policy = "UserRolePolicy")]
        public RedirectToActionResult AddRating(int id)
        {
            _repository.AddRating(id);
            return RedirectToAction("ListCountry");
        }

    }
}