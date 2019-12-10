using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;

namespace SongsProject.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly ISongRepository repository;

        public NavigationMenuViewComponent(ISongRepository repo)
        {
            repository = repo;
        }

        //renders the default Razor partial view
        public IViewComponentResult Invoke()
        { 
            //indicate which country has been selected
            ViewBag.SelectedCountry = RouteData?.Values["Country"];
            return View(repository.Songs
                .Select(x => x.Country)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}