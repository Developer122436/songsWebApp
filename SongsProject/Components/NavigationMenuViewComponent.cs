using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using System.Linq;

namespace SongsProject.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly ISongRepository repository;

        public NavigationMenuViewComponent(ISongRepository repo)
        {
            repository = repo;
        }

        //Renders the navigation menu partial view
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCountry = RouteData?.Values["Country"];
            return View(repository.Songs
                .Select(x => x.Country)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}