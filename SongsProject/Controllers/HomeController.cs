using Microsoft.AspNetCore.Authorization;
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
        private readonly ISongRepository _repo;
        private readonly ILogger _logger;


        public HomeController(ISongRepository repo, ILogger<HomeController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public ViewResult ListCountry(string Country, int songPage = 1)
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
                Songs = _repo.Songs
                    .Where(p => Country == null || p.Country == Country)
                    .OrderByDescending(p => p.Rating)
                    .Skip((songPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = songPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Country == null
                        ? _repo.Songs.Count()
                        : _repo.Songs.Count(e => e.Country == Country)
                },
                CurrentCountry = Country
            });
        }

        public ViewResult ListNames(string Name, int songPage = 1)
        {
            return View(new SongsListViewModel
            {
                Songs = _repo.Songs
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
                        ? _repo.Songs.Count()
                        : _repo.Songs.Count(e => e.Name == Name)
                },
                CurrentName = Name
            });
        }

        [Authorize(Policy = "UserRolePolicy")]
        public RedirectToActionResult AddRating(int id)
        {
            _repo.AddRating(id);
            return RedirectToAction("ListCountry");
        }

    }
}