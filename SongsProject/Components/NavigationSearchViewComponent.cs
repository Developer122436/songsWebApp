using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;

namespace SongsProject.Components
{
    public class NavigationSearchViewComponent : ViewComponent
    {
        private readonly ISongRepository repository;

        public NavigationSearchViewComponent(ISongRepository repo)
        {
            repository = repo;
        }

        //renders the default Razor partial view
        public IViewComponentResult Invoke()
        {
            //Instead of writing ViewData["SelectedName"]
            //We write ViewBag.SelectedName - instead string key we use
            //dynamic properties.

            //indicate which country has been selected
            ViewBag.SelectedName = RouteData?.Values["Name"];
            return View(repository.Songs
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}